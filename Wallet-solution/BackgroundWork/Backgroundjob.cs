using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Impl;
using Wallet_solution.Models;
using Wallet_solution.Services;

namespace Wallet_solution.BackgroundWork
{
    public class Backgroundjob
    {
        private readonly InterestService _interestService;


        public Backgroundjob(InterestService _interestService)
        {
            this._interestService = _interestService;
        }
        public async Task ScheduleInterestJob()
        {
            JobDataMap jobDataMap = new JobDataMap();

            jobDataMap.Add(nameof(InterestService), _interestService);
            
            try
            {
                StdSchedulerFactory factory = new StdSchedulerFactory();
                IScheduler scheduler = await factory.GetScheduler();


                await scheduler.Start();

                IJobDetail job = JobBuilder.Create<InterestJob>()
                    .WithIdentity("IntJob1", "group1")
                    .UsingJobData(jobDataMap)
                    .Build();
                

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .WithCronSchedule("0 0 24 * * ?")
                    .ForJob("IntJob1", "group1")
                    .Build();

                await scheduler.ScheduleJob(job, trigger);

            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }

        }
    }

    public class InterestJob : IJob
    {

        public async Task Execute(IJobExecutionContext context)
        {

            JobDataMap jobData = context.MergedJobDataMap;

            var service =  (InterestService)jobData[nameof(InterestService)];

            var walletCount = await service.WalletYealyInterest();

            Console.WriteLine();

            Console.WriteLine($"- - - > Interest given to {walletCount} wallet < - - -");

        }
    }
}
