using BullOak.Repositories.EventStore.Test.Integration.Components;
using BullOak.Repositories.EventStore.Test.Integration.Ids;
using BullOak.Repositories.Repository;

[assembly: Xunit.CollectionBehavior(DisableTestParallelization = true)]

namespace BullOak.Repositories.EventStore.Test.Integration.StepDefinitions
{
    using BoDi;
    using BullOak.Repositories.EventStore.Test.Integration.Contexts;
    using System;
    using System.Threading.Tasks;
    using TechTalk.SpecFlow;

    [Binding]
    public class TestsSetupAndTeardown
    {
        private readonly IObjectContainer objectContainer;

        public TestsSetupAndTeardown(ScenarioContext context, IObjectContainer objectContainer)
        {
            this.objectContainer = objectContainer ?? throw new ArgumentNullException(nameof(objectContainer));
        }

        [BeforeScenario]
        public void BootstrapObjectContainer()
        {
            var config = InProcEventStoreIntegrationContext.CreateConfigurationForBullOak();
            objectContainer.RegisterInstanceAs<IStartSessions<ImplicitlySerializableId, IHoldHigherOrder>>(new EventStoreRepository<ImplicitlySerializableId, IHoldHigherOrder>(
                config, InProcEventStoreIntegrationContext.GetConnection()));
            objectContainer.RegisterInstanceAs<IStartSessions<ExplicitlySerializableId, IHoldHigherOrder>>(new EventStoreRepository<ExplicitlySerializableId, IHoldHigherOrder>(
                config, InProcEventStoreIntegrationContext.GetConnection()));

        }

        [BeforeTestRun]
        public static Task BeforeTestRun()
        {
            return InProcEventStoreIntegrationContext.SetupNode();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            InProcEventStoreIntegrationContext.TeardownNode();
        }
        
    }
}
