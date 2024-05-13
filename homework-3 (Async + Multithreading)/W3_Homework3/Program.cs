// See https://aka.ms/new-console-template for more information

using System.Threading.Channels;
using Microsoft.Extensions.Configuration;
using W3_Homework3;

class Program
{
    public static async Task Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        IConfiguration config = builder.Build();
        
        var settings = config.GetSection("ParallelConfiguration").Get<ParallelSettings>();
        // Управление степенью параллелизма расчета через конфиг (как сделать с IOptionsMonitor в консольном приложении не понял (нужен DI контейнер вроде, либо в ASP делать))
        int threadsCount = settings.ThreadsCount;
        Semaphore semaphore = new(threadsCount, threadsCount);
    
        var exePath = AppDomain.CurrentDomain.BaseDirectory;
        
        var inputPath = Path.Combine(exePath, "input.txt");
        using var reader = new StreamReader(inputPath);
        reader.ReadLine(); // delete first line with column names
        
        var outputPath = Path.Combine(exePath, "output.txt");
        using var writer = new StreamWriter(outputPath);
        writer.WriteLine("id, demand");

        int readRowsCount = 0;
        int evaluatedRowsCount = 0;
        int wroteRowsCount = 0;
        
        var readChannel = Channel.CreateBounded<ProductInfo>(10);
        
        var tasks = new List<Task>();
        
        var tokenSource = new CancellationTokenSource();
        CancellationToken ct = tokenSource.Token;
        
        var readProduceTask = Task.Run(async () =>
        {
            while (!reader.EndOfStream)
            {
                if (ct.IsCancellationRequested)
                    break;
                var row = reader.ReadLine().Split(", ").Select(int.Parse).ToArray();
                var productInfo = new ProductInfo(row[0], row[1], row[2]);
                Console.WriteLine($"Thread id: {Thread.CurrentThread.ManagedThreadId} Прочитано строк: {++readRowsCount}. " +
                                  $"Последняя: id = {productInfo.Id}, prediction = {productInfo.Prediction}, stock = {productInfo.Stock}");
                await readChannel.Writer.WriteAsync(productInfo);
                await Task.Delay(100);
            }
            readChannel.Writer.Complete();
        }, ct);
        
        var readConsumeTask = Task.Run(async () =>
        {
            await foreach (var productInfo in readChannel.Reader.ReadAllAsync())
            {
                tasks.Add(Task.Run(() =>
                {
                    if (ct.IsCancellationRequested)
                        return;
                    // var demandInfo = Evaluating.Evaluate(productInfo);
                    // await writeChannel.Writer.WriteAsync(demandInfo);
                    var demandInfo = Evaluate(productInfo, semaphore, ref evaluatedRowsCount, ct);
                    if (demandInfo is null)
                        return;
                    writer.WriteLine($"{productInfo.Id}, {Math.Max(productInfo.Prediction - productInfo.Stock, 0)}");
                    Console.WriteLine($"Thread id: {Thread.CurrentThread.ManagedThreadId} Записано строк: {++wroteRowsCount}. " +
                                               $"Последняя: id = {demandInfo.Id}, demand = {demandInfo.Demand}");
                }, ct));
            }
        });
        
        // var writeConsumeTask = Task.Run(async () =>
        // {
        //     await foreach (var demandInfo in writeChannel.Reader.ReadAllAsync())
        //     {
        //         writer.WriteLine($"{demandInfo.Id}, {demandInfo.Demand}");
        //         Console.WriteLine($"Thread id: {Thread.CurrentThread.ManagedThreadId} Записана строка {demandInfo.RowNumber}: " +
        //                           $"id = {demandInfo.Id}, demand = {demandInfo.Demand}");
        //     }
        // });

        var thread = new Thread(() =>
        {
            Console.ReadKey(true);
            Console.WriteLine("Расчет отменён");
            tokenSource.Cancel();
            tasks.Clear();
        })
        {
            IsBackground = true
        };
        thread.Start();
        
        await Task.WhenAll(readProduceTask, readConsumeTask);
        await Task.WhenAll(tasks.ToArray());
    }
    
    public static DemandInfo Evaluate(ProductInfo productInfo, Semaphore semaphore, ref int evaluatedRowsCount, CancellationToken ct)
    {
        semaphore.WaitOne();
        
        // Имитация сложности вычислительной операции в академических целях (просто крутим цикл)
        int loopIterations = Random.Shared.Next(1000000000, 2000000000);
        for (int i = 0; i < loopIterations; ++i)
        {
            if (ct.IsCancellationRequested)
            {
                return null;
            }
        };

        int demand = Math.Max(productInfo.Prediction - productInfo.Stock, 0);
        Console.WriteLine($"Thread id: {Thread.CurrentThread.ManagedThreadId} Обработано строк: {++evaluatedRowsCount}. " +
                          $"Последняя: id = {productInfo.Id}, demand = {demand}");
        semaphore.Release();
        return new DemandInfo(productInfo.Id, demand);
    }
}

public record ProductInfo(int Id, int Prediction, int Stock);
public record DemandInfo(int Id, int Demand);

// public static class Evaluating
// {
//     private const int _threadsCount = 5;
//     private static readonly Semaphore _semaphore = new(_threadsCount, _threadsCount);
//
//     private static int _evaluatedRowsCount;
//     
//     public static DemandInfo Evaluate(ProductInfo productInfo)
//     {
//         _semaphore.WaitOne();
//         
//         // Имитация сложности вычислительной операции в академических целях (просто крутим цикл)
//         int loopIterations = Random.Shared.Next(1000000000, 2000000000);
//         for (int i = 0; i < loopIterations; ++i);
//
//         int demand = Math.Max(productInfo.Prediction - productInfo.Stock, 0);
//         Console.WriteLine($"Thread id: {Thread.CurrentThread.ManagedThreadId} Обработано строк: {++_evaluatedRowsCount}. " +
//                           $"Последняя: id = {productInfo.Id}, demand = {demand}");
//         _semaphore.Release();
//         return new DemandInfo(productInfo.Id, demand);
//     }
// }