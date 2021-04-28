// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CvrTests.cs" company="Clued In">
//   Copyright (c) 2019 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements the cvr tests class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using CluedIn.Core.Data;
using CluedIn.Core.Data.Parts;
using CluedIn.Core.Messages.Processing;
using CluedIn.Core.Processing;
using CluedIn.Core.Serialization;
using CluedIn.Core.Workflows;
using CluedIn.ExternalSearch;
using CluedIn.ExternalSearch.Providers.CVR;
using CluedIn.Testing.Base.Context;
using CluedIn.Testing.Base.Processing.Actors;
using Moq;
using Xunit;

namespace ExternalSearch.CVR.Integration.Tests
{
    public class CvrBitTests
    {
        [Fact(Skip = "GitHub Issue 829 - ref https://github.com/CluedIn-io/CluedIn/issues/829")]
        public void CvrNumberTest()
        {
            // Arrange
            var testContext = new TestContext();
            var properties = new EntityMetadataPart();
            properties.Properties.Add(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.CodesCVR, "31579864");
            IEntityMetadata entityMetadata = new EntityMetadataPart()
            {
                Name        = "Sitecore",
                EntityType  = EntityType.Organization,
                Properties  =  properties.Properties

            };

            var externalSearchProvider  = new Mock<CvrExternalSearchProvider>(MockBehavior.Loose);
            var clues                   = new List<CompressedClue>();

            externalSearchProvider.CallBase = true;

            testContext.ProcessingHub.Setup(h => h.SendCommand(It.IsAny<ProcessClueCommand>())).Callback<IProcessingCommand>(c => clues.Add(((ProcessClueCommand)c).Clue));
            testContext.Container.Register(Component.For<IExternalSearchProvider>().UsingFactoryMethod(() => externalSearchProvider.Object));

            var context         = testContext.Context.ToProcessingContext();
            var command         = new ExternalSearchCommand();
            var actor           = new ExternalSearchProcessingAccessor(context.ApplicationContext);
            var workflow        = new Mock<Workflow>(MockBehavior.Loose, context, new EmptyWorkflowTemplate<ExternalSearchCommand>());

            workflow.CallBase = true;

            command.With(context);
            command.OrganizationId = context.Organization.Id;
            command.EntityMetaData = entityMetadata;
            command.Workflow       = workflow.Object;
            context.Workflow       = command.Workflow;

            // Act
            var result = actor.ProcessWorkflowStep(context, command);
            Assert.Equal(WorkflowStepResult.Repeat.SaveResult, result.SaveResult);

            result = actor.ProcessWorkflowStep(context, command);
            Assert.Equal(WorkflowStepResult.Success.SaveResult, result.SaveResult);
            context.Workflow.AddStepResult(result);

            context.Workflow.ProcessStepResult(context, command);

            // Assert
            testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);

            Assert.True(clues.Count > 0);
        }

        [Fact(Skip = "GitHub Issue 829 - ref https://github.com/CluedIn-io/CluedIn/issues/829")]
        public void NameTest()
        {
            // Arrange
            var testContext = new TestContext();
            var properties = new EntityMetadataPart();

            IEntityMetadata entityMetadata = new EntityMetadataPart()
            {
                Name        = "Sitecore International A/S",
                EntityType  = EntityType.Organization,
                Properties  =  properties.Properties

            };

            var externalSearchProvider  = new Mock<CvrExternalSearchProvider>(MockBehavior.Loose);
            var clues                   = new List<CompressedClue>();

            externalSearchProvider.CallBase = true;

            testContext.ProcessingHub.Setup(h => h.SendCommand(It.IsAny<ProcessClueCommand>())).Callback<IProcessingCommand>(c => clues.Add(((ProcessClueCommand)c).Clue));
            testContext.Container.Register(Component.For<IExternalSearchProvider>().UsingFactoryMethod(() => externalSearchProvider.Object));

            var context         = testContext.Context.ToProcessingContext();
            var command         = new ExternalSearchCommand();
            var actor           = new ExternalSearchProcessingAccessor(context.ApplicationContext);
            var workflow        = new Mock<Workflow>(MockBehavior.Loose, context, new EmptyWorkflowTemplate<ExternalSearchCommand>());

            workflow.CallBase = true;

            command.With(context);
            command.OrganizationId = context.Organization.Id;
            command.EntityMetaData = entityMetadata;
            command.Workflow       = workflow.Object;
            context.Workflow       = command.Workflow;

            // Act
            var result = actor.ProcessWorkflowStep(context, command);
            Assert.Equal(WorkflowStepResult.Repeat.SaveResult, result.SaveResult);

            result = actor.ProcessWorkflowStep(context, command);
            Assert.Equal(WorkflowStepResult.Success.SaveResult, result.SaveResult);
            context.Workflow.AddStepResult(result);

            context.Workflow.ProcessStepResult(context, command);

            // Assert
            testContext.ProcessingHub.Verify(h => h.SendCommand(It.IsAny<ProcessClueCommand>()), Times.AtLeastOnce);

            Assert.True(clues.Count > 0);
        }
    }
}
