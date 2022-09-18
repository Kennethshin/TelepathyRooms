using TelepathyRooms.Services.BookingServices;
using TelepathyRooms.ViewModel;

namespace TelepathyUnitTest
{
    [TestClass]
    public class BookingServiceTest
    {
        BookingService _bookingService;

        [TestInitialize]
        public void TestInitialize()
        {
            _bookingService = new BookingService();
        }

        [TestMethod]
        public void GetAscendingSequence_NullRoom_IsNull()
        {
            //Arrange
            List<HotelRoom> mockList = null;

            //Act
            var result = _bookingService.GetRoomPath(mockList);

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAscendingSequence_RoomPath_Success()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", OrderNumber = 0, FloorNo = 1 },
                new HotelRoom() { RoomNumber = "1B", OrderNumber = 0, FloorNo = 1 },
                new HotelRoom() { RoomNumber = "1C", OrderNumber = 0, FloorNo = 1 },
                new HotelRoom() { RoomNumber = "2A", OrderNumber = 0, FloorNo = 2 },
                new HotelRoom() { RoomNumber = "2B", OrderNumber = 0, FloorNo = 2 },
                new HotelRoom() { RoomNumber = "2C", OrderNumber = 0, FloorNo = 2 }
            };

            var expected = new List<HotelRoom>()
             {
                new HotelRoom() { RoomNumber = "1A", OrderNumber = 1 },
                new HotelRoom() { RoomNumber = "1B", OrderNumber = 2 },
                new HotelRoom() { RoomNumber = "1C", OrderNumber = 3 },
                new HotelRoom() { RoomNumber = "2A", OrderNumber = 6 },
                new HotelRoom() { RoomNumber = "2B", OrderNumber = 5 },
                new HotelRoom() { RoomNumber = "2C", OrderNumber = 4 }
            };

            //Act
            var result = _bookingService.GetRoomPath(mockList);
            var orderedResult = result?.OrderBy(x => x.OrderNumber).Select(x => x.RoomNumber).ToList();
            bool equal = orderedResult.SequenceEqual(expected.OrderBy(x => x.OrderNumber).Select(x => x.RoomNumber).ToList());
            
            //Assert
            Assert.IsTrue(equal);
        }

        [TestMethod]
        public void GetAllAvailableRooms_NullRooms_IsNull()
        {
            //Arrange
            List<HotelRoom> mockList = null;

            //Act
            var result = _bookingService.GetAllAvailableRooms(mockList);

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllAvailableRooms_AvailableHotelRooms_Success()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "1B", Status = RoomStatus.Repair },
                new HotelRoom() { RoomNumber = "1C", Status = RoomStatus.Available },
                new HotelRoom() { RoomNumber = "2A", Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2B", Status = RoomStatus.Available },
                new HotelRoom() { RoomNumber = "2C", Status = RoomStatus.Vacant }
            };

            var expected = new List<String>()
            {
                "1C",
                "2B"
            };

            //Act
            var result = _bookingService.GetAllAvailableRooms(mockList).Select(x => x.RoomNumber);
            bool equal = result.SequenceEqual(expected);

            //Assert
            Assert.IsTrue(equal);
        }

        [TestMethod]
        public void Assign_NullRooms_EmptyString()
        {
            //Arrange
            List<HotelRoom> mockList = null;

            //Act
            var result = _bookingService.AssignRoom(mockList);

            //Assert
            Assert.AreEqual(String.Empty,result);
        }

        [TestMethod]
        public void Assign_HasAvailableHotelRooms_Success()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "1B", Status = RoomStatus.Repair },
                new HotelRoom() { RoomNumber = "1C", Status = RoomStatus.Available },
                new HotelRoom() { RoomNumber = "2A", Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2B", Status = RoomStatus.Available },
                new HotelRoom() { RoomNumber = "2C", Status = RoomStatus.Vacant }
            };

            var expected = "1C";

            //Act
            var result = _bookingService.AssignRoom(mockList);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Assign_NoAvailableHotelRooms_Unsuccessful()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "1B", Status = RoomStatus.Repair },
                new HotelRoom() { RoomNumber = "1C", Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "2A", Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2B", Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2C", Status = RoomStatus.Vacant }
            };

            var expected = String.Empty;

            //Act
            var result = _bookingService.AssignRoom(mockList);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Assign_AvailableStatusChangeToOccupied_Success()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "1B", Status = RoomStatus.Repair },
                new HotelRoom() { RoomNumber = "1C", Status = RoomStatus.Available },
                new HotelRoom() { RoomNumber = "2A", Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2B", Status = RoomStatus.Available },
                new HotelRoom() { RoomNumber = "2C", Status = RoomStatus.Vacant }
            };

            var expected = RoomStatus.Occupied;

            //Act
            _bookingService.AssignRoom(mockList);
            var result = mockList.Where(x => x.RoomNumber == "1C").First().Status;

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Assign_NonAvailableStatusChangeToOccupied_Unsuccessful()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "1B", Status = RoomStatus.Repair },
                new HotelRoom() { RoomNumber = "1C", Status = RoomStatus.Available },
                new HotelRoom() { RoomNumber = "2A", Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2B", Status = RoomStatus.Available },
                new HotelRoom() { RoomNumber = "2C", Status = RoomStatus.Vacant }
            };

            var expected = RoomStatus.Occupied;

            //Act
            _bookingService.AssignRoom(mockList);
            var result = mockList.Where(x => x.RoomNumber == "1A").First().Status;

            //Assert
            Assert.AreNotEqual(expected, result);
        }

        [TestMethod]
        public void CheckOutRoom_NullRoom_Unsuccessful()
        {
            //Arrange
            List<HotelRoom> mockList = null;

            //Act
            var result = _bookingService.CheckOutRoom(mockList, "2A");

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckOutRoom_HasOccupiedRoom_Successful()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "1B", FloorNo = 1, Status = RoomStatus.Repair },
                new HotelRoom() { RoomNumber = "1C", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "2A", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2B", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2C", FloorNo = 2, Status = RoomStatus.Vacant }
            };

            var expected = true;

            //Act
            var result = _bookingService.CheckOutRoom(mockList, "2A");

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CheckOutRoom_NonOccupiedRoom_Unsuccessful()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "1B", FloorNo = 1, Status = RoomStatus.Repair },
                new HotelRoom() { RoomNumber = "1C", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "2A", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2B", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2C", FloorNo = 2, Status = RoomStatus.Vacant }
            };

            var expected = false;

            //Act
            var result = _bookingService.CheckOutRoom(mockList, "1A");

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CheckOut_OccupiedStatusChangeToVacant_Success()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "1B", FloorNo = 1, Status = RoomStatus.Repair },
                new HotelRoom() { RoomNumber = "1C", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "2A", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2B", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2C", FloorNo = 2, Status = RoomStatus.Vacant }
            };

            var expected = RoomStatus.Vacant;

            //Act
            _bookingService.CheckOutRoom(mockList, "2A");
            var result = mockList.Where(x => x.RoomNumber == "2A").First().Status;

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CheckOut_NonOccupiedStatusChangeToVacant_Unsuccessful()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "1B", FloorNo = 1, Status = RoomStatus.Repair },
                new HotelRoom() { RoomNumber = "1C", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "2A", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2B", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2C", FloorNo = 2, Status = RoomStatus.Vacant }
            };

            var expected = RoomStatus.Vacant;

            //Act
            _bookingService.CheckOutRoom(mockList, "1B");
            var result = mockList.Where(x => x.RoomNumber == "1B").First().Status;

            //Assert
            Assert.AreNotEqual(expected, result);
        }

        [TestMethod]
        public void CleanRoom_NullRoom_Unsucessful()
        {
            //Arrange
            List<HotelRoom> mockList = null;

            //Act
            var result = _bookingService.CleanRoom(mockList, "1C");

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CleanRoom_HasVacantRoom_Successful()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "1B", FloorNo = 1, Status = RoomStatus.Repair },
                new HotelRoom() { RoomNumber = "1C", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "2A", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2B", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2C", FloorNo = 2, Status = RoomStatus.Vacant }
            };

            var expected = true;

            //Act
            var result = _bookingService.CleanRoom(mockList, "1C");

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CleanRoom_NonVacantRoom_Unsuccessful()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "1B", FloorNo = 1, Status = RoomStatus.Repair },
                new HotelRoom() { RoomNumber = "1C", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "2A", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2B", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2C", FloorNo = 2, Status = RoomStatus.Vacant }
            };

            var expected = false;

            //Act
            var result = _bookingService.CleanRoom(mockList, "1B");

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CleanRoom_VacantStatusChangeToAvailable_Success()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "1B", FloorNo = 1, Status = RoomStatus.Repair },
                new HotelRoom() { RoomNumber = "1C", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "2A", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2B", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2C", FloorNo = 2, Status = RoomStatus.Vacant }
            };

            var expected = RoomStatus.Available;

            //Act
            _bookingService.CleanRoom(mockList, "1C");
            var result = mockList.Where(x => x.RoomNumber == "1C").First().Status;

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CleanRoom_NonVacantStatusChangeToAvailable_Unsuccessful()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "1B", FloorNo = 1, Status = RoomStatus.Repair },
                new HotelRoom() { RoomNumber = "1C", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "2A", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2B", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2C", FloorNo = 2, Status = RoomStatus.Vacant }
            };

            var expected = RoomStatus.Available;

            //Act
            _bookingService.CleanRoom(mockList, "2B");
            var result = mockList.Where(x => x.RoomNumber == "2B").First().Status;

            //Assert
            Assert.AreNotEqual(expected, result);
        }

        [TestMethod]
        public void OutOfServiceRoom_NullRoom_Unsuccessful()
        {
            //Arrange
            List<HotelRoom> mockList = null;

            //Act
            var result = _bookingService.OutOfServiceRoom(mockList, "1C");

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void OutOfServiceRoom_HasVacantRoom_Successful()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "1B", FloorNo = 1, Status = RoomStatus.Repair },
                new HotelRoom() { RoomNumber = "1C", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "2A", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2B", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2C", FloorNo = 2, Status = RoomStatus.Vacant }
            };

            var expected = true;

            //Act
            var result = _bookingService.OutOfServiceRoom(mockList, "1C");

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void OutOfServiceRoom_NonVacantRoom_Unsuccessful()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "1B", FloorNo = 1, Status = RoomStatus.Repair },
                new HotelRoom() { RoomNumber = "1C", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "2A", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2B", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2C", FloorNo = 2, Status = RoomStatus.Vacant }
            };

            var expected = false;

            //Act
            var result = _bookingService.OutOfServiceRoom(mockList, "1B");

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void OutOfServiceRoom_VacantStatusChangeToRepair_Success()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "1B", FloorNo = 1, Status = RoomStatus.Repair },
                new HotelRoom() { RoomNumber = "1C", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "2A", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2B", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2C", FloorNo = 2, Status = RoomStatus.Vacant }
            };

            var expected = RoomStatus.Repair;

            //Act
            _bookingService.OutOfServiceRoom(mockList, "1C");
            var result = mockList.Where(x => x.RoomNumber == "1C").First().Status;

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void OutOfServiceRoom_NonVacantStatusChangeToRepair_Unsuccessful()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "1B", FloorNo = 1, Status = RoomStatus.Repair },
                new HotelRoom() { RoomNumber = "1C", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "2A", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2B", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2C", FloorNo = 2, Status = RoomStatus.Vacant }
            };

            var expected = RoomStatus.Repair;

            //Act
            _bookingService.OutOfServiceRoom(mockList, "2B");
            var result = mockList.Where(x => x.RoomNumber == "2B").First().Status;

            //Assert
            Assert.AreNotEqual(expected, result);
        }

        [TestMethod]
        public void RepairedRoom_NullRoom_Unsuccessful()
        {
            //Arrange
            List<HotelRoom> mockList = null;

            //Act
            var result = _bookingService.RepairedRoom(mockList, "1B");

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RepairedRoom_HasRepairRoom_Successful()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "1B", FloorNo = 1, Status = RoomStatus.Repair },
                new HotelRoom() { RoomNumber = "1C", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "2A", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2B", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2C", FloorNo = 2, Status = RoomStatus.Vacant }
            };

            var expected = true;

            //Act
            var result = _bookingService.RepairedRoom(mockList, "1B");

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RepairedRoom_NonRepairRoom_Unsuccessful()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "1B", FloorNo = 1, Status = RoomStatus.Repair },
                new HotelRoom() { RoomNumber = "1C", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "2A", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2B", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2C", FloorNo = 2, Status = RoomStatus.Vacant }
            };

            var expected = false;

            //Act
            var result = _bookingService.RepairedRoom(mockList, "2C");

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RepairedRoom_RepairStatusChangeToVacant_Success()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "1B", FloorNo = 1, Status = RoomStatus.Repair },
                new HotelRoom() { RoomNumber = "1C", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "2A", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2B", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2C", FloorNo = 2, Status = RoomStatus.Vacant }
            };

            var expected = RoomStatus.Vacant;

            //Act
            _bookingService.RepairedRoom(mockList, "1B");
            var result = mockList.Where(x => x.RoomNumber == "1B").First().Status;

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RepairedRoom_NonRepairStatusChangeToVacant_Unsuccessful()
        {
            //Arrange
            var mockList = new List<HotelRoom>()
            {
                new HotelRoom() { RoomNumber = "1A", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "1B", FloorNo = 1, Status = RoomStatus.Repair },
                new HotelRoom() { RoomNumber = "1C", FloorNo = 1, Status = RoomStatus.Vacant },
                new HotelRoom() { RoomNumber = "2A", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2B", FloorNo = 2, Status = RoomStatus.Occupied },
                new HotelRoom() { RoomNumber = "2C", FloorNo = 2, Status = RoomStatus.Vacant }
            };

            var expected = RoomStatus.Repair;

            //Act
            _bookingService.RepairedRoom(mockList, "2B");
            var result = mockList.Where(x => x.RoomNumber == "2B").First().Status;

            //Assert
            Assert.AreNotEqual(expected, result);
        }
    }
}