
using Quartz;
using Quartz.Impl;
using Wallet_solution.Services;

namespace Wallet_solution.BackgroundWork
{
    public class Backgroundjob
    {
        private readonly InterestService _interestService;
        private readonly IServiceScopeFactory _factory;



        public Backgroundjob(InterestService _interestService, IServiceScopeFactory _factory)
        {
            this._interestService = _interestService;
            this._factory = _factory;
        }
        public async Task ProducerJob()
        {
            JobDataMap jobDataMap = new JobDataMap();

            jobDataMap.Add(nameof(InterestService), _interestService);
            
            try
            {
                StdSchedulerFactory factory = new StdSchedulerFactory();
                IScheduler scheduler = await factory.GetScheduler();


                await scheduler.Start();

                IJobDetail producerJob = JobBuilder.Create<ProducerJob>()
                    .WithIdentity(nameof(producerJob), "group1")
                    .UsingJobData(jobDataMap)
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .WithCronSchedule("0 0 23 * * ?")
                    .ForJob(nameof(producerJob), "group1")
                    .Build();

                await scheduler.ScheduleJob(producerJob, trigger);


            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }

        }


        public async Task ConsumerJob()
        {
            JobDataMap jobDataMap = new JobDataMap();

            jobDataMap.Add(nameof(InterestService), _interestService);

            try
            {
                StdSchedulerFactory factory = new StdSchedulerFactory();
                IScheduler scheduler = await factory.GetScheduler();


                IJobDetail consumerJob = JobBuilder.Create<ConsumerJob>()
                    .WithIdentity(nameof(consumerJob), "group2")
                    .UsingJobData(jobDataMap)
                    .Build();

                ITrigger consumerTrigger = TriggerBuilder.Create()
                    .WithIdentity("trigger2", "group1")
                    .WithCronSchedule("0 0 23 * * ?")
                    .ForJob(nameof(consumerJob), "group2")
                    .Build();
                await scheduler.ScheduleJob(consumerJob, consumerTrigger);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }

        }

    }

    public class ProducerJob : IJob
    {

        public async Task Execute(IJobExecutionContext context)
        {

            JobDataMap jobData = context.MergedJobDataMap;

            var service =  (InterestService)jobData[nameof(InterestService)];

            await service.WalletYealyInterest();

            Console.WriteLine();
        }
    }

    public class ConsumerJob : IJob
    {

        public async Task Execute(IJobExecutionContext context)
        {

            JobDataMap jobData = context.MergedJobDataMap;

            var service = (InterestService)jobData[nameof(InterestService)];

            await service.ReceiveWalletForInterest("interestQueue");


            Console.WriteLine($"Interest Processed");

            Console.WriteLine();


        }
    }


}
