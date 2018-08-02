using Moq;
using RestaurantAPI.API.Controllers;
using RestaurantAPI.Library.Repos;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace RestaurantAPI.Testing
{
    public class RestaurantControllerTest
    {
        public RestaurantControllerTest()
        {
            mockARepo = new Mock<IAppUserRepo>();
            mockKRepo = new Mock<KeywordRepo>();
            mockQRepo = new Mock<QueryRepo>();
            mockRRepo = new Mock<IRestaurantRepo>();
            controller = new RestaurantController(
               mockARepo.Object, mockKRepo.Object, mockQRepo.Object, mockRRepo.Object);
        }

        private readonly Mock<IAppUserRepo> mockARepo;
        private readonly Mock<KeywordRepo> mockKRepo;
        private readonly Mock<QueryRepo> mockQRepo;
        private readonly Mock<IRestaurantRepo> mockRRepo;
        private readonly RestaurantController controller;


       /* [Fact]
        public void getpassesiflistisnotempty()
        {
            //Arrange
            mockRRepo.Setup(x => x.GetRestaurantByID).Returns.ID;

            //Act
            var result = controller.Get();

            //Assert
            Assert.IsType;
        }


        [Fact]
        public void getfailsiflistisempty()
        {
            //Arrange
            mockRRepo.Setup(x => x.)
            //Act

            //Assert
        }

        [Fact]
        public void createpassesif()
        {
            //Arrange

            //Act

            //Assert

        }
        */
    }
}
