using System;
using System.Collections.Generic;
using FluentAssertions;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Aggregates;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Enumerations;
using Riva.Announcements.Domain.RoomForRentAnnouncements.Exceptions;
using Xunit;

namespace Riva.Announcements.Domain.Test
{
    public class RoomForRentAnnouncementTest
    {
        [Fact]
        public void Should_Create_RoomForRentAnnouncement()
        {
            var result = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> {Guid.NewGuid()})
                .SetRoomTypes(new List<RoomTypeEnumeration> {RoomTypeEnumeration.Single})
                .Build();
                

            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Throw_RoomForRentAnnouncementTitleNullException_When_Title_Is_Null_Or_Empty(string title)
        {
            Action result = () =>
            {
                var unused = RoomForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle(title)
                    .SetSourceUrl("http://source")
                    .SetCityId(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription("Description")
                    .SetPrice(1000)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementTitleNullException>()
                .WithMessage("Title argument is required.");
        }

        [Fact]
        public void Should_Throw_RoomForRentAnnouncementTitleMaxLengthException_When_Title_Exceeds_Allowed_Max_Length_Value()
        {
            var title = CreateString(257);
            Action result = () =>
            {
                var unused = RoomForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle(title)
                    .SetSourceUrl("http://source")
                    .SetCityId(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription("Description")
                    .SetPrice(1000)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementTitleMaxLengthException>()
                .WithMessage("Title argument max length is 256.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Throw_RoomForRentAnnouncementSourceUrlNullException_When_SourceUrl_Is_Null_Or_Empty(string sourceUrl)
        {
            Action result = () =>
            {
                var unused = RoomForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle("Title")
                    .SetSourceUrl(sourceUrl)
                    .SetCityId(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription("Description")
                    .SetPrice(1000)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementSourceUrlNullException>()
                .WithMessage("SourceUrl argument is required.");
        }

        [Fact]
        public void Should_Throw_RoomForRentAnnouncementCityIdInvalidValueException_When_CityId_Is_New_Guid()
        {
            Action result = () =>
            {
                var unused = RoomForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle("Title")
                    .SetSourceUrl("http://source")
                    .SetCityId(new Guid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription("Description")
                    .SetPrice(1000)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementCityIdInvalidValueException>()
                .WithMessage("CityId argument is invalid.");
        }

        [Fact]
        public void Should_Throw_RoomForRentAnnouncementCityIdInvalidValueException_When_CityId_Is_Empty_Guid()
        {
            Action result = () =>
            {
                var unused = RoomForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle("Title")
                    .SetSourceUrl("http://source")
                    .SetCityId(Guid.Empty)
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription("Description")
                    .SetPrice(1000)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementCityIdInvalidValueException>()
                .WithMessage("CityId argument is invalid.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Throw_RoomForRentAnnouncementDescriptionNullException_When_Description_Is_Null_Or_Empty(string description)
        {
            Action result = () =>
            {
                var unused = RoomForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle("Title")
                    .SetSourceUrl("http://source")
                    .SetCityId(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription(description)
                    .SetPrice(1000)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementDescriptionNullException>()
                .WithMessage("Description argument is required.");
        }

        [Fact]
        public void Should_Throw_RoomForRentAnnouncementCityDistrictsNullException_When_CityDistricts_Is_Null()
        {
            Action result = () =>
            {
                var unused = RoomForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle("Title")
                    .SetSourceUrl("http://source")
                    .SetCityId(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription("Description")
                    .SetPrice(1000)
                    .SetCityDistricts(null)
                    .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementCityDistrictsNullException>()
                .WithMessage("CityDistricts argument is required.");
        }

        [Fact]
        public void Should_Throw_RoomForRentAnnouncementCityDistrictsInvalidValueException_When_CityDistricts_Contains_New_Guid()
        {
            Action result = () =>
            {
                var unused = RoomForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle("Title")
                    .SetSourceUrl("http://source")
                    .SetCityId(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription("Description")
                    .SetPrice(1000)
                    .SetCityDistricts(new List<Guid> { new Guid() })
                    .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementCityDistrictsInvalidValueException>()
                .WithMessage("CityDistricts argument is invalid.");
        }

        [Fact]
        public void Should_Throw_RoomForRentAnnouncementCityDistrictsInvalidValueException_When_CityDistricts_Contains_Empty_Guid()
        {
            Action result = () =>
            {
                var unused = RoomForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle("Title")
                    .SetSourceUrl("http://source")
                    .SetCityId(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription("Description")
                    .SetPrice(1000)
                    .SetCityDistricts(new List<Guid> { Guid.Empty })
                    .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementCityDistrictsInvalidValueException>()
                .WithMessage("CityDistricts argument is invalid.");
        }

        [Fact]
        public void Should_Throw_RoomForRentAnnouncementCityDistrictsDuplicateValuesException_When_CityDistricts_Contains_Duplicate()
        {
            var cityDistricts = Guid.NewGuid();
            Action result = () =>
            {
                var unused = RoomForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle("Title")
                    .SetSourceUrl("http://source")
                    .SetCityId(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription("Description")
                    .SetPrice(1000)
                    .SetCityDistricts(new List<Guid> { cityDistricts, cityDistricts })
                    .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementCityDistrictsDuplicateValuesException>()
                .WithMessage("CityDistricts argument contains duplicate values.");
        }

        [Fact]
        public void Should_Throw_RoomForRentAnnouncementRoomTypesNullException_When_RoomTypes_Is_Null()
        {
            Action result = () =>
            {
                var unused = RoomForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle("Title")
                    .SetSourceUrl("http://source")
                    .SetCityId(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription("Description")
                    .SetPrice(1000)
                    .SetCityDistricts(new List<Guid>{Guid.NewGuid()})
                    .SetRoomTypes(null)
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementRoomTypesNullException>()
                .WithMessage("RoomTypes argument is required.");
        }

        [Fact]
        public void Should_Throw_RoomForRentAnnouncementRoomTypesNullException_When_RoomTypes_Contains_Null()
        {
            Action result = () =>
            {
                var unused = RoomForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle("Title")
                    .SetSourceUrl("http://source")
                    .SetCityId(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription("Description")
                    .SetPrice(1000)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .SetRoomTypes(new List<RoomTypeEnumeration>{null})
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementRoomTypesNullException>()
                .WithMessage("RoomTypes argument is required.");
        }

        [Fact]
        public void Should_Throw_RoomForRentAnnouncementRoomTypesDuplicateValuesException_When_RoomTypes_Contains_Duplicate()
        {
            var roomType = RoomTypeEnumeration.Single;
            Action result = () =>
            {
                var unused = RoomForRentAnnouncement.Builder()
                    .SetId(Guid.NewGuid())
                    .SetTitle("Title")
                    .SetSourceUrl("http://source")
                    .SetCityId(Guid.NewGuid())
                    .SetCreated(DateTimeOffset.UtcNow)
                    .SetDescription("Description")
                    .SetPrice(1000)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .SetRoomTypes(new List<RoomTypeEnumeration> { roomType, roomType })
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementRoomTypesDuplicateValuesException>()
                .WithMessage("RoomTypes argument contains duplicate values.");
        }

        [Fact]
        public void ChangeTitle_Should_Change_Title()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                .Build();
            const string newTitle = "NewTitle";

            roomForRentAnnouncement.ChangeTitle(newTitle);

            roomForRentAnnouncement.Title.Should().Be(newTitle);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ChangeTitle_Should_Throw_RoomForRentAnnouncementTitleNullException_When_Title_Is_Null_Or_Empty(string title)
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> {Guid.NewGuid()})
                .SetRoomTypes(new List<RoomTypeEnumeration> {RoomTypeEnumeration.Single})
                .Build();

            Action result = () => roomForRentAnnouncement.ChangeTitle(title);

            result.Should().ThrowExactly<RoomForRentAnnouncementTitleNullException>()
                .WithMessage("Title argument is required.");
        }

        [Fact]
        public void ChangeTitle_Should_Throw_RoomForRentAnnouncementTitleMaxLengthException_When_Title_Exceeds_Allowed_Max_Length_Value()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                .Build();
            var title = CreateString(257);

            Action result = () => roomForRentAnnouncement.ChangeTitle(title);

            result.Should().ThrowExactly<RoomForRentAnnouncementTitleMaxLengthException>()
                .WithMessage("Title argument max length is 256.");
        }

        [Fact]
        public void ChangeSourceUrl_Should_Change_SourceUrl()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                .Build();
            const string newSourceUrl = "http://newSourceUrl";

            roomForRentAnnouncement.ChangeSourceUrl(newSourceUrl);

            roomForRentAnnouncement.SourceUrl.Should().Be(newSourceUrl);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ChangeSourceUrl_Should_Throw_RoomForRentAnnouncementSourceUrlNullException_When_SourceUrl_Is_Null_Or_Empty(string sourceUrl)
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                .Build();

            Action result = () => roomForRentAnnouncement.ChangeSourceUrl(sourceUrl);

            result.Should().ThrowExactly<RoomForRentAnnouncementSourceUrlNullException>()
                .WithMessage("SourceUrl argument is required.");
        }

        [Fact]
        public void ChangeCityId_Should_Change_CityId()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                .Build();
            var newCityId = Guid.NewGuid();

            roomForRentAnnouncement.ChangeCityId(newCityId);

            roomForRentAnnouncement.CityId.Should().Be(newCityId);
        }

        [Fact]
        public void ChangeCityId_Should_Throw_RoomForRentAnnouncementCityIdInvalidValueException_When_CityId_Is_New_Guid()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                .Build();

            Action result = () => roomForRentAnnouncement.ChangeCityId(new Guid());

            result.Should().ThrowExactly<RoomForRentAnnouncementCityIdInvalidValueException>()
                .WithMessage("CityId argument is invalid.");
        }

        [Fact]
        public void ChangeCityId_Should_Throw_RoomForRentAnnouncementCityIdInvalidValueException_When_CityId_Is_Empty_Guid()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                .Build();

            Action result = () => roomForRentAnnouncement.ChangeCityId(Guid.Empty);

            result.Should().ThrowExactly<RoomForRentAnnouncementCityIdInvalidValueException>()
                .WithMessage("CityId argument is invalid.");
        }

        [Fact]
        public void ChangeDescription_Should_Change_Description()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                .Build();
            const string newDescription = "New description";

            roomForRentAnnouncement.ChangeDescription(newDescription);

            roomForRentAnnouncement.Description.Should().Be(newDescription);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ChangeDescription_Should_Throw_RoomForRentAnnouncementDescriptionNullException_When_Description_Is_Null_Or_Empty(string description)
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                .Build();

            Action result = () => roomForRentAnnouncement.ChangeDescription(description);

            result.Should().ThrowExactly<RoomForRentAnnouncementDescriptionNullException>()
                .WithMessage("Description argument is required.");
        }

        [Fact]
        public void AddCityDistrict_Should_Add_CityDistrict()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                .Build();
            var newCityDistrict = Guid.NewGuid();

            roomForRentAnnouncement.AddCityDistrict(newCityDistrict);

            roomForRentAnnouncement.CityDistricts.Should().Contain(newCityDistrict);
        }

        [Fact]
        public void AddCityDistrict_Should_Throw_RoomForRentAnnouncementCityDistrictNullException_When_CityDistrict_Is_New_Guid()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                .Build();

            Action result = () => roomForRentAnnouncement.AddCityDistrict(new Guid());

            result.Should().ThrowExactly<RoomForRentAnnouncementCityDistrictNullException>()
                .WithMessage("CityDistrict argument is required.");
        }

        [Fact]
        public void AddCityDistrict_Should_Throw_RoomForRentAnnouncementCityDistrictNullException_When_CityDistrict_Is_Empty_Guid()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                .Build();

            Action result = () => roomForRentAnnouncement.AddCityDistrict(Guid.Empty);

            result.Should().ThrowExactly<RoomForRentAnnouncementCityDistrictNullException>()
                .WithMessage("CityDistrict argument is required.");
        }

        [Fact]
        public void RemoveCityDistrict_Should_Remove_CityDistrict()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                .Build();
            var cityDistrict = Guid.NewGuid();

            roomForRentAnnouncement.RemoveCityDistrict(cityDistrict);

            roomForRentAnnouncement.CityDistricts.Should().NotContain(cityDistrict);
        }

        [Fact]
        public void RemoveCityDistrict_Should_Throw_RoomForRentAnnouncementCityDistrictNullException_When_CityDistrict_Is_New_Guid()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                .Build();

            Action result = () => roomForRentAnnouncement.RemoveCityDistrict(new Guid());

            result.Should().ThrowExactly<RoomForRentAnnouncementCityDistrictNullException>()
                .WithMessage("CityDistrict argument is required.");
        }

        [Fact]
        public void RemoveCityDistrict_Should_Throw_RoomForRentAnnouncementCityDistrictNullException_When_CityDistrict_Is_Empty_Guid()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                .Build();

            Action result = () => roomForRentAnnouncement.RemoveCityDistrict(Guid.Empty);

            result.Should().ThrowExactly<RoomForRentAnnouncementCityDistrictNullException>()
                .WithMessage("CityDistrict argument is required.");
        }

        [Fact]
        public void AddRoomType_Should_Add_RoomType()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                .Build();
            var newRoomType = RoomTypeEnumeration.Double;

            roomForRentAnnouncement.AddRoomType(newRoomType);

            roomForRentAnnouncement.RoomTypes.Should().Contain(newRoomType);
        }

        [Fact]
        public void AddRoomType_Should_Throw_RoomForRentAnnouncementRoomTypeNullException_When_RoomType_Is_Null()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                .Build();

            Action result = () => roomForRentAnnouncement.AddRoomType(null);

            result.Should().ThrowExactly<RoomForRentAnnouncementRoomTypeNullException>()
                .WithMessage("RoomType argument is required.");
        }

        [Fact]
        public void RemoveRoomType_Should_Remove_RoomType()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                .Build();
            var roomType = RoomTypeEnumeration.Double;

            roomForRentAnnouncement.RemoveRoomType(roomType);

            roomForRentAnnouncement.RoomTypes.Should().NotContain(roomType);
        }

        [Fact]
        public void RemoveRoomType_Should_Throw_RoomForRentAnnouncementRoomTypeNullException_When_RoomType_Is_Null()
        {
            var roomForRentAnnouncement = RoomForRentAnnouncement.Builder()
                .SetId(Guid.NewGuid())
                .SetTitle("Title")
                .SetSourceUrl("http://source")
                .SetCityId(Guid.NewGuid())
                .SetCreated(DateTimeOffset.UtcNow)
                .SetDescription("Description")
                .SetPrice(1000)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .SetRoomTypes(new List<RoomTypeEnumeration> { RoomTypeEnumeration.Single })
                .Build();

            Action result = () => roomForRentAnnouncement.RemoveRoomType(null);

            result.Should().ThrowExactly<RoomForRentAnnouncementRoomTypeNullException>()
                .WithMessage("RoomType argument is required.");
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