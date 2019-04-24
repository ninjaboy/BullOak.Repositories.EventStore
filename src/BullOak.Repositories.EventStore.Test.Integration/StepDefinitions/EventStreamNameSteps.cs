using System;
using System.Threading.Tasks;
using BullOak.Repositories.EventStore.Test.Integration.Components;
using BullOak.Repositories.EventStore.Test.Integration.Contexts;
using BullOak.Repositories.EventStore.Test.Integration.Ids;
using BullOak.Repositories.Repository;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace BullOak.Repositories.EventStore.Test.Integration.StepDefinitions
{
    
    [Binding]
    public class EventStreamNameSteps
    {
        private readonly StreamNameContext streamNameContext;
        private readonly IStartSessions<ImplicitlySerializableId, IHoldHigherOrder> implicitIdRepository;
        private readonly IStartSessions<ExplicitlySerializableId, IHoldHigherOrder> explicitIdRepository;
        private readonly InProcEventStoreIntegrationContext eventStoreContainer;

        public EventStreamNameSteps(StreamNameContext streamNameContext, 
            IStartSessions<ImplicitlySerializableId, IHoldHigherOrder> implicitIdRepository,
            IStartSessions<ExplicitlySerializableId, IHoldHigherOrder> explicitIdRepository,
            InProcEventStoreIntegrationContext eventStoreContainer)
        {
            this.streamNameContext = streamNameContext ?? throw new ArgumentNullException(nameof(streamNameContext));
            this.implicitIdRepository = implicitIdRepository ?? throw new ArgumentNullException(nameof(implicitIdRepository));
            this.explicitIdRepository = explicitIdRepository?? throw new ArgumentNullException(nameof(explicitIdRepository));
            this.eventStoreContainer = eventStoreContainer ?? throw new ArgumentNullException(nameof(eventStoreContainer));
        }
        
        
        [Given(@"a new Id explicitly implementing ISerializeToStreamName")]
        public void GivenAnExplicitlySerializableId()
        {
            streamNameContext.LastGeneratedId = Guid.NewGuid();
            streamNameContext.ExplicitlySerializableId = new ExplicitlySerializableId(streamNameContext.LastGeneratedId);
            streamNameContext.ExplicitlySerializableIdExpected = true;
        }
        
        [Given(@"a new Id that doesn't implement ISerializeToStreamName")]
        public void GivenAnImplicitlySerializableId()
        {
            streamNameContext.LastGeneratedId = Guid.NewGuid();
            streamNameContext.ImplicitlySerializableId = new ImplicitlySerializableId(streamNameContext.LastGeneratedId);
            streamNameContext.ExplicitlySerializableIdExpected = false;
        }

        
        [When(@"I try to save the new events for that id")]
        public async Task WhenITryToSaveTheNewEventsForThatId()
        {
            if (streamNameContext.ExplicitlySerializableIdExpected)
            {
                using (var session = await explicitIdRepository.BeginSessionFor(streamNameContext.ExplicitlySerializableId, false))
                {
                    var highOrder = session.GetCurrentState();
                    session.AddEvent(new MyEvent(1));
                    await session.SaveChanges();
                }
            }
            else
            {
                using (var session = await implicitIdRepository.BeginSessionFor(streamNameContext.ImplicitlySerializableId, false))
                {
                    var highOrder = session.GetCurrentState();
                    session.AddEvent(new MyEvent(1));
                    await session.SaveChanges();
                }
            }
        }

        [Then(@"the explicitly named event stream should exist and contain saved data")]
        public async Task ThenExplicitlyNamedStreamExists()
        {
            var events = await eventStoreContainer.ReadEventsFromStreamRaw(streamNameContext.ExplicitlySerializableId.Serialize());
            events.Length.Should().Be(1);
            
            var events2 = await eventStoreContainer.ReadEventsFromStreamRaw(streamNameContext.ExplicitlySerializableId.ToString());
            events2.Should().BeEmpty();

        }
        
        [Then(@"the ToString based event stream should exist and contain saved data")]
        public async Task ThenTheToStringBasedStreamExists()
        {
            var events = await eventStoreContainer.ReadEventsFromStreamRaw(streamNameContext.ImplicitlySerializableId.ToString());
            events.Length.Should().Be(1);
            
            var events2 = await eventStoreContainer.ReadEventsFromStreamRaw(streamNameContext.ImplicitlySerializableId.Serialize());
            events2.Should().BeEmpty();

        }
    }
}