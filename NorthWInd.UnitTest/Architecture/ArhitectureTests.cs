using NetArchTest.Rules;


namespace NorthWInd.UnitTest.Architecture
{
    public class ArhitectureTests
    {
        private const string DomainNamespace = "NorthWind.Entities";
        private const string ApplicationBONamespace = "NorthWind.Sales.BusinessObjects";
        private const string ApplicationUCNamespace = "NorthWind.Sales.UseCases";
        private const string InfrastructureEFNamespace = "NorthWind.EFCore.Repositories";
        private const string IoCNamespace = "NorthWind.Sales.IoC";
        private const string WebApiNamespace = "NorthWind.Sales.API";

        [Fact]
        public void Domain_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            var otherProjects = new[]
            {
                ApplicationBONamespace,
                ApplicationUCNamespace,
                InfrastructureEFNamespace,
                IoCNamespace,
                WebApiNamespace
            };

            // Act
            var testResult = Types.InCurrentDomain()
                .That()
                .ResideInNamespace(DomainNamespace)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert
            Assert.True(testResult.IsSuccessful);
        }

        [Fact]
        public void Application_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            var otherProjects = new[]
            {
                InfrastructureEFNamespace,
                IoCNamespace,
                WebApiNamespace
            };

            // Act
            var testResult = Types.InCurrentDomain()
                .That()
                .ResideInNamespace(ApplicationUCNamespace)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert
            Assert.True(testResult.IsSuccessful);
        }

        [Fact]
        public void IoC_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            var otherProjects = new[]
            {
                WebApiNamespace
            };

            // Act
            var testResult = Types.InCurrentDomain()
                .That()
                .ResideInNamespace(IoCNamespace)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert
            Assert.True(testResult.IsSuccessful);
        }

        [Fact]
        public void Validate_Rules_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            var otherProjects = new[]
            {
                DomainNamespace,
                ApplicationBONamespace,
                ApplicationUCNamespace,
                InfrastructureEFNamespace,
                WebApiNamespace
            };

            // Act
            var testResult = Types.InCurrentDomain()
                .That()
                .ResideInNamespace(IoCNamespace)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert
            Assert.False(testResult.IsSuccessful);
        }


        //[Fact]
        //public void Handlers_Should_Have_DependencyOnDomain()
        //{
        //    // Arrange
        //    var assembly = typeof(Application.AssemblyReference).Assembly;

        //    // Act
        //    var testResult = Types
        //        .InAssembly(assembly)
        //        .That()
        //        .HaveNameEndingWith("Handler")
        //        .Should()
        //        .HaveDependencyOn(DomainNamespace)
        //        .GetResult();

        //    // Assert
        //    Assert.True(testResult.IsSuccessful);
        //}

    }
}
