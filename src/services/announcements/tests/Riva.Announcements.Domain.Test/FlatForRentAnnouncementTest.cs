using System;
using System.Collections.Generic;
using FluentAssertions;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Enumerations;
using Riva.Announcements.Domain.FlatForRentAnnouncements.Exceptions;
using Xunit;

namespace Riva.Announcements.Domain.Test
{
    public class FlatForRentAnnouncementTest
    {
        [Fact]
        public void Should_Create_FlatForRentAnnouncement()
        {
            var result = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> {Guid.NewGuid()})
                .Build();
                

            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Throw_FlatForRentAnnouncementTitleNullException_When_Title_Is_Null_Or_Empty(string title)
        {
            Action result = () =>
            {
                var unused = FlatForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle(title)
                    .SetSourceUrl("http://source")
                    .SetCityId(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription("Description")
                    .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                    .SetPrice(1000)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementTitleNullException>()
                .WithMessage("Title argument is required.");
        }

        [Fact]
        public void Should_Throw_FlatForRentAnnouncementTitleMaxLengthException_When_Title_Exceeds_Allowed_Max_Length_Value()
        {
            var title = CreateString(257);
            Action result = () =>
            {
                var unused = FlatForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle(title)
                    .SetSourceUrl("http://source")
                    .SetCityId(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription("Description")
                    .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                    .SetPrice(1000)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementTitleMaxLengthException>()
                .WithMessage("Title argument max length is 256.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Throw_FlatForRentAnnouncementSourceUrlNullException_When_SourceUrl_Is_Null_Or_Empty(string sourceUrl)
        {
            Action result = () =>
            {
                var unused = FlatForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle("Title")
                    .SetSourceUrl(sourceUrl)
                    .SetCityId(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription("Description")
                    .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                    .SetPrice(1000)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementSourceUrlNullException>()
                .WithMessage("SourceUrl argument is required.");
        }

        [Fact]
        public void Should_Throw_FlatForRentAnnouncementCityIdInvalidValueException_When_CityId_Is_New_Guid()
        {
            Action result = () =>
            {
                var unused = FlatForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle("Title")
                    .SetSourceUrl("http://source")
                    .SetCityId(new Guid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription("Description")
                    .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                    .SetPrice(1000)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementCityIdInvalidValueException>()
                .WithMessage("CityId argument is invalid.");
        }

        [Fact]
        public void Should_Throw_FlatForRentAnnouncementCityIdInvalidValueException_When_CityId_Is_Empty_Guid()
        {
            Action result = () =>
            {
                var unused = FlatForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle("Title")
                    .SetSourceUrl("http://source")
                    .SetCityId(Guid.Empty)
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription("Description")
                    .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                    .SetPrice(1000)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementCityIdInvalidValueException>()
                .WithMessage("CityId argument is invalid.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Throw_FlatForRentAnnouncementDescriptionNullException_When_Description_Is_Null_Or_Empty(string description)
        {
            Action result = () =>
            {
                var unused = FlatForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle("Title")
                    .SetSourceUrl("http://source")
                    .SetCityId(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription(description)
                    .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                    .SetPrice(1000)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementDescriptionNullException>()
                .WithMessage("Description argument is required.");
        }

        [Fact]
        public void Should_Throw_FlatForRentAnnouncementCityDistrictsNullException_When_CityDistricts_Is_Null()
        {
            Action result = () =>
            {
                var unused = FlatForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle("Title")
                    .SetSourceUrl("http://source")
                    .SetCityId(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription("Description")
                    .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                    .SetPrice(1000)
                    .SetCityDistricts(null)
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementCityDistrictsNullException>()
                .WithMessage("CityDistricts argument is required.");
        }

        [Fact]
        public void Should_Throw_FlatForRentAnnouncementCityDistrictsInvalidValueException_When_CityDistricts_Contains_New_Guid()
        {
            Action result = () =>
            {
                var unused = FlatForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle("Title")
                    .SetSourceUrl("http://source")
                    .SetCityId(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription("Description")
                    .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                    .SetPrice(1000)
                    .SetCityDistricts(new List<Guid> { new Guid() })
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementCityDistrictsInvalidValueException>()
                .WithMessage("CityDistricts argument is invalid.");
        }

        [Fact]
        public void Should_Throw_FlatForRentAnnouncementCityDistrictsInvalidValueException_When_CityDistricts_Contains_Empty_Guid()
        {
            Action result = () =>
            {
                var unused = FlatForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle("Title")
                    .SetSourceUrl("http://source")
                    .SetCityId(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription("Description")
                    .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                    .SetPrice(1000)
                    .SetCityDistricts(new List<Guid> { Guid.Empty })
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementCityDistrictsInvalidValueException>()
                .WithMessage("CityDistricts argument is invalid.");
        }

        [Fact]
        public void Should_Throw_FlatForRentAnnouncementCityDistrictsDuplicateValuesException_When_CityDistricts_Contains_Duplicate()
        {
            var cityDistricts = Guid.NewGuid();
            Action result = () =>
            {
                var unused = FlatForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle("Title")
                    .SetSourceUrl("http://source")
                    .SetCityId(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription("Description")
                    .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                    .SetPrice(1000)
                    .SetCityDistricts(new List<Guid> { cityDistricts, cityDistricts })
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementCityDistrictsDuplicateValuesException>()
                .WithMessage("CityDistricts argument contains duplicate values.");
        }

        [Fact]
        public void ChangeTitle_Should_Change_Title()
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            const string newTitle = "NewTitle";

            flatForRentAnnouncement.ChangeTitle(newTitle);

            flatForRentAnnouncement.Title.Should().Be(newTitle);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ChangeTitle_Should_Throw_FlatForRentAnnouncementTitleNullException_When_Title_Is_Null_Or_Empty(string title)
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> {Guid.NewGuid()})
                .Build();

            Action result = () => flatForRentAnnouncement.ChangeTitle(title);

            result.Should().ThrowExactly<FlatForRentAnnouncementTitleNullException>()
                .WithMessage("Title argument is required.");
        }

        [Fact]
        public void ChangeTitle_Should_Throw_FlatForRentAnnouncementTitleMaxLengthException_When_Title_Exceeds_Allowed_Max_Length_Value()
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var title = CreateString(257);

            Action result = () => flatForRentAnnouncement.ChangeTitle(title);

            result.Should().ThrowExactly<FlatForRentAnnouncementTitleMaxLengthException>()
                .WithMessage("Title argument max length is 256.");
        }

        [Fact]
        public void ChangeSourceUrl_Should_Change_SourceUrl()
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            const string newSourceUrl = "http://newSourceUrl";

            flatForRentAnnouncement.ChangeSourceUrl(newSourceUrl);

            flatForRentAnnouncement.SourceUrl.Should().Be(newSourceUrl);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ChangeSourceUrl_Should_Throw_FlatForRentAnnouncementSourceUrlNullException_When_SourceUrl_Is_Null_Or_Empty(string sourceUrl)
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => flatForRentAnnouncement.ChangeSourceUrl(sourceUrl);

            result.Should().ThrowExactly<FlatForRentAnnouncementSourceUrlNullException>()
                .WithMessage("SourceUrl argument is required.");
        }

        [Fact]
        public void ChangeCityId_Should_Change_CityId()
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var newCityId = Guid.NewGuid();

            flatForRentAnnouncement.ChangeCityId(newCityId);

            flatForRentAnnouncement.CityId.Should().Be(newCityId);
        }

        [Fact]
        public void ChangeCityId_Should_Throw_FlatForRentAnnouncementCityIdInvalidValueException_When_CityId_Is_New_Guid()
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => flatForRentAnnouncement.ChangeCityId(new Guid());

            result.Should().ThrowExactly<FlatForRentAnnouncementCityIdInvalidValueException>()
                .WithMessage("CityId argument is invalid.");
        }

        [Fact]
        public void ChangeCityId_Should_Throw_FlatForRentAnnouncementCityIdInvalidValueException_When_CityId_Is_Empty_Guid()
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => flatForRentAnnouncement.ChangeCityId(Guid.Empty);

            result.Should().ThrowExactly<FlatForRentAnnouncementCityIdInvalidValueException>()
                .WithMessage("CityId argument is invalid.");
        }

        [Fact]
        public void ChangeDescription_Should_Change_Description()
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            const string newDescription = "New description";

            flatForRentAnnouncement.ChangeDescription(newDescription);

            flatForRentAnnouncement.Description.Should().Be(newDescription);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ChangeDescription_Should_Throw_FlatForRentAnnouncementDescriptionNullException_When_Description_Is_Null_Or_Empty(string description)
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => flatForRentAnnouncement.ChangeDescription(description);

            result.Should().ThrowExactly<FlatForRentAnnouncementDescriptionNullException>()
                .WithMessage("Description argument is required.");
        }

        [Fact]
        public void ChangeChangePrice_Should_Change_ChangePrice()
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var newPrice = 2000;

            flatForRentAnnouncement.ChangePrice(newPrice);

            flatForRentAnnouncement.Price.Should().Be(newPrice);
        }

        [Fact]
        public void ChangeNumberOfRooms_Should_Change_NumberOfRooms()
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var newNumberOfRooms = NumberOfRoomsEnumeration.FiveAndMore;

            flatForRentAnnouncement.ChangeNumberOfRooms(newNumberOfRooms);

            flatForRentAnnouncement.NumberOfRooms.Should().BeEquivalentTo(newNumberOfRooms);
        }

        [Fact]
        public void AddCityDistrict_Should_Add_CityDistrict()
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var newCityDistrict = Guid.NewGuid();

            flatForRentAnnouncement.AddCityDistrict(newCityDistrict);

            flatForRentAnnouncement.CityDistricts.Should().Contain(newCityDistrict);
        }

        [Fact]
        public void AddCityDistrict_Should_Throw_FlatForRentAnnouncementCityDistrictNullException_When_CityDistrict_Is_New_Guid()
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => flatForRentAnnouncement.AddCityDistrict(new Guid());

            result.Should().ThrowExactly<FlatForRentAnnouncementCityDistrictNullException>()
                .WithMessage("CityDistrict argument is required.");
        }

        [Fact]
        public void AddCityDistrict_Should_Throw_FlatForRentAnnouncementCityDistrictNullException_When_CityDistrict_Is_Empty_Guid()
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => flatForRentAnnouncement.AddCityDistrict(Guid.Empty);

            result.Should().ThrowExactly<FlatForRentAnnouncementCityDistrictNullException>()
                .WithMessage("CityDistrict argument is required.");
        }

        [Fact]
        public void RemoveCityDistrict_Should_Remove_CityDistrict()
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var cityDistrict = Guid.NewGuid();

            flatForRentAnnouncement.RemoveCityDistrict(cityDistrict);

            flatForRentAnnouncement.CityDistricts.Should().NotContain(cityDistrict);
        }

        [Fact]
        public void RemoveCityDistrict_Should_Throw_FlatForRentAnnouncementCityDistrictNullException_When_CityDistrict_Is_New_Guid()
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => flatForRentAnnouncement.RemoveCityDistrict(new Guid());

            result.Should().ThrowExactly<FlatForRentAnnouncementCityDistrictNullException>()
                .WithMessage("CityDistrict argument is required.");
        }

        [Fact]
        public void RemoveCityDistrict_Should_Throw_FlatForRentAnnouncementCityDistrictNullException_When_CityDistrict_Is_Empty_Guid()
        {
            var flatForRentAnnouncement = FlatForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetNumberOfRooms(NumberOfRoomsEnumeration.One)
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => flatForRentAnnouncement.RemoveCityDistrict(Guid.Empty);

            result.Should().ThrowExactly<FlatForRentAnnouncementCityDistrictNullException>()
                .WithMessage("CityDistrict argument is required.");
        }

        private static string CreateString(int charNumber)
        {
            var secretHash = string.Empty;
            for (var i = 0; i < charNumber; i++)
            {
                secretHash += "a";
            }

            return secretHash;
        }
    }
}