using System;
using System.Collections.Generic;
using FluentAssertions;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Domain.Users.Enumerations;
using Riva.Users.Domain.Users.Exceptions.EntityExceptions.RoomForRentAnnouncementPreferenceExceptions;
using Xunit;

namespace Riva.Users.Domain.Test
{
    public class RoomForRentAnnouncementPreferenceTest
    {
        [Fact]
        public void Should_Create_RoomForRentAnnouncementPreference()
        {
            var roomForRentAnnouncementPreferenceId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var cityDistricts = new List<Guid> { Guid.NewGuid() };
            const decimal priceMin = 0;
            const decimal priceMax = 2000;
            var roomType = RoomTypeEnumeration.Single;
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(roomForRentAnnouncementPreferenceId)
                .SetCityId(cityId)
                .SetPriceMin(priceMin)
                .SetPriceMax(priceMax)
                .SetRoomType(roomType)
                .SetCityDistricts(cityDistricts)
                .Build();

            roomForRentAnnouncementPreference.Id.Should().Be(roomForRentAnnouncementPreferenceId);
            roomForRentAnnouncementPreference.CityId.Should().Be(cityId);
            roomForRentAnnouncementPreference.PriceMin.Should().Be(priceMin);
            roomForRentAnnouncementPreference.PriceMax.Should().Be(priceMax);
            roomForRentAnnouncementPreference.RoomType.Should().Be(roomType);
            roomForRentAnnouncementPreference.CityDistricts.Should().BeEquivalentTo(cityDistricts);
        }

        [Fact]
        public void Should_Create_RoomForRentAnnouncementPreference_With_Null_Values()
        {
            var roomForRentAnnouncementPreferenceId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var cityDistricts = new List<Guid> { Guid.NewGuid() };
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(roomForRentAnnouncementPreferenceId)
                .SetCityId(cityId)
                .SetPriceMin(null)
                .SetPriceMax(null)
                .SetRoomType(null)
                .SetCityDistricts(cityDistricts)
                .Build();

            roomForRentAnnouncementPreference.Id.Should().Be(roomForRentAnnouncementPreferenceId);
            roomForRentAnnouncementPreference.CityId.Should().Be(cityId);
            roomForRentAnnouncementPreference.PriceMin.Should().BeNull();
            roomForRentAnnouncementPreference.PriceMax.Should().BeNull();
            roomForRentAnnouncementPreference.RoomType.Should().BeNull();
            roomForRentAnnouncementPreference.CityDistricts.Should().BeEquivalentTo(cityDistricts);
        }

        [Fact]
        public void Should_Throw_RoomForRentAnnouncementPreferenceIdNullException_When_Id_Is_Empty_Guid()
        {
            Action result = () =>
            {
                var unused = RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.Empty)
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(new List<Guid> {Guid.NewGuid()})
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementPreferenceIdNullException>()
                .WithMessage("Id argument is required.");
        }

        [Fact]
        public void Should_Throw_RoomForRentAnnouncementPreferenceIdNullException_When_Id_Is_New_Guid()
        {
            Action result = () =>
            {
                var unused = RoomForRentAnnouncementPreference.Builder()
                    .SetId(new Guid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementPreferenceIdNullException>()
                .WithMessage("Id argument is required.");
        }

        [Fact]
        public void Should_Throw_RoomForRentAnnouncementPreferenceCityIdNullException_When_CityId_Is_Empty_Guid()
        {
            Action result = () =>
            {
                var unused = RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.Empty)
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementPreferenceCityIdNullException>()
                .WithMessage("CityId argument is required.");
        }

        [Fact]
        public void Should_Throw_RoomForRentAnnouncementPreferenceCityIdNullException_When_CityId_Is_New_Guid()
        {
            Action result = () =>
            {
                var unused = RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(new Guid())
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementPreferenceCityIdNullException>()
                .WithMessage("CityId argument is required.");
        }

        [Fact]
        public void Should_Throw_RoomForRentAnnouncementPreferencePriceMinMinValueException_When_PriceMin_Is_Negative_Value()
        {
            Action result = () =>
            {
                var unused = RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(-1)
                    .SetPriceMax(2000)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementPreferencePriceMinMinValueException>()
                .WithMessage("PriceMin argument min value is 0.");
        }

        [Fact]
        public void Should_Throw_RoomForRentAnnouncementPreferencePriceMaxMinValueException_When_PriceMax_Is_Negative_Value()
        {
            Action result = () =>
            {
                var unused = RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(-1)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementPreferencePriceMaxMinValueException>()
                .WithMessage("PriceMax argument min value is 0.");
        }

        [Fact]
        public void Should_Throw_RoomForRentAnnouncementPreferencePriceMaxLowerThanPriceMinException_When_PriceMax_Is_Lower_Than_PriceMin()
        {
            Action result = () =>
            {
                var unused = RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(1000)
                    .SetPriceMax(0)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementPreferencePriceMaxLowerThanPriceMinException>()
                .WithMessage("PriceMax argument cannot be lower than PriceMin.");
        }

        [Fact]
        public void Should_Throw_RoomForRentAnnouncementPreferenceCityDistrictsNullException_When_CityDistricts_Is_Null()
        {
            Action result = () =>
            {
                var unused = RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(null)
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementPreferenceCityDistrictsNullException>()
                .WithMessage("CityDistricts argument is required.");
        }

        [Fact]
        public void Should_Throw_RoomForRentAnnouncementPreferenceCityDistrictsNullException_When_CityDistricts_Contains_Empty_Guid()
        {
            Action result = () =>
            {
                var unused = RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(new List<Guid> {Guid.Empty})
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementPreferenceCityDistrictsNullException>()
                .WithMessage("CityDistricts argument is required.");
        }

        [Fact]
        public void Should_Throw_RoomForRentAnnouncementPreferenceCityDistrictsNullException_When_CityDistricts_Contains_New_Guid()
        {
            Action result = () =>
            {
                var unused = RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(new List<Guid> { new Guid() })
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementPreferenceCityDistrictsNullException>()
                .WithMessage("CityDistricts argument is required.");
        }

        [Fact]
        public void Should_Throw_RoomForRentAnnouncementPreferenceCityDistrictsDuplicateValuesException_When_CityDistricts_Contains_Duplicates()
        {
            var cityDistrict = Guid.NewGuid();
            Action result = () =>
            {
                var unused = RoomForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomType(RoomTypeEnumeration.Single)
                    .SetCityDistricts(new List<Guid> { cityDistrict, cityDistrict })
                    .Build();
            };

            result.Should().ThrowExactly<RoomForRentAnnouncementPreferenceCityDistrictsDuplicateValuesException>()
                .WithMessage("CityDistricts argument contains duplicate values.");
        }

        [Fact]
        public void ChangeCityId_Should_Change_CityId()
        {
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var newCityId = Guid.NewGuid();

            roomForRentAnnouncementPreference.ChangeCityId(newCityId);

            roomForRentAnnouncementPreference.CityId.Should().Be(newCityId);
        }

        [Fact]
        public void ChangeCityId_Should_Throw_RoomForRentAnnouncementPreferenceCityIdNullException_When_CityId_Is_Empty_Guid()
        {
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => roomForRentAnnouncementPreference.ChangeCityId(Guid.Empty);

            result.Should().ThrowExactly<RoomForRentAnnouncementPreferenceCityIdNullException>()
                .WithMessage("CityId argument is required.");
        }

        [Fact]
        public void ChangeCityId_Should_Throw_RoomForRentAnnouncementPreferenceCityIdNullException_When_CityId_Is_New_Guid()
        {
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => roomForRentAnnouncementPreference.ChangeCityId(new Guid());

            result.Should().ThrowExactly<RoomForRentAnnouncementPreferenceCityIdNullException>()
                .WithMessage("CityId argument is required.");
        }

        [Fact]
        public void ChangePriceMin_Should_Change_PriceMin()
        {
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            roomForRentAnnouncementPreference.ChangePriceMin(null);

            roomForRentAnnouncementPreference.PriceMin.Should().BeNull();
        }

        [Fact]
        public void ChangePriceMin_Should_Throw_RoomForRentAnnouncementPreferencePriceMinMinValueException_When_PriceMin_Is_Negative_Value()
        {
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => roomForRentAnnouncementPreference.ChangePriceMin(-1);

            result.Should().ThrowExactly<RoomForRentAnnouncementPreferencePriceMinMinValueException>()
                .WithMessage("PriceMin argument min value is 0.");
        }

        [Fact]
        public void ChangePriceMax_Should_Change_PriceMax()
        {
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            roomForRentAnnouncementPreference.ChangePriceMax(null);

            roomForRentAnnouncementPreference.PriceMax.Should().BeNull();
        }

        [Fact]
        public void ChangePriceMax_Should_Throw_RoomForRentAnnouncementPreferencePriceMaxMinValueException_When_PriceMax_Is_Negative_Value()
        {
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => roomForRentAnnouncementPreference.ChangePriceMax(-1);

            result.Should().ThrowExactly<RoomForRentAnnouncementPreferencePriceMaxMinValueException>()
                .WithMessage("PriceMax argument min value is 0.");
        }

        [Fact]
        public void ChangePriceMax_Should_Throw_RoomForRentAnnouncementPreferencePriceMaxLowerThanPriceMinException_When_PriceMax_Is_Lower_Than_PriceMin()
        {
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(1000)
                .SetPriceMax(2000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => roomForRentAnnouncementPreference.ChangePriceMax(0);

            result.Should().ThrowExactly<RoomForRentAnnouncementPreferencePriceMaxLowerThanPriceMinException>()
                .WithMessage("PriceMax argument cannot be lower than PriceMin.");
        }

        [Fact]
        public void ChangeRoomType_Should_Change_RoomType()
        {
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var newRoomType = RoomTypeEnumeration.Double;

            roomForRentAnnouncementPreference.ChangeRoomType(newRoomType);

            roomForRentAnnouncementPreference.RoomType.Should().BeEquivalentTo(newRoomType);
        }

        [Fact]
        public void ChangeCityDistricts_Should_Change_CityDistricts()
        {
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var newCityDistricts = new List<Guid> { Guid.NewGuid() };

            roomForRentAnnouncementPreference.ChangeCityDistricts(newCityDistricts);

            roomForRentAnnouncementPreference.CityDistricts.Should().BeEquivalentTo(newCityDistricts);
        }

        [Fact]
        public void ChangeCityDistricts_Should_Throw_RoomForRentAnnouncementPreferenceCityDistrictsNullException_When_CityDistricts_Is_Null()
        {
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => roomForRentAnnouncementPreference.ChangeCityDistricts(null);

            result.Should().ThrowExactly<RoomForRentAnnouncementPreferenceCityDistrictsNullException>()
                .WithMessage("CityDistricts argument is required.");
        }

        [Fact]
        public void ChangeCityDistricts_Should_Throw_RoomForRentAnnouncementPreferenceCityDistrictsNullException_When_CityDistricts_Contains_Empty_Guid()
        {
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => roomForRentAnnouncementPreference.ChangeCityDistricts(new List<Guid> { Guid.Empty });

            result.Should().ThrowExactly<RoomForRentAnnouncementPreferenceCityDistrictsNullException>()
                .WithMessage("CityDistricts argument is required.");
        }

        [Fact]
        public void ChangeCityDistricts_Should_Throw_RoomForRentAnnouncementPreferenceCityDistrictsNullException_When_CityDistricts_Contains_New_Guid()
        {
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => roomForRentAnnouncementPreference.ChangeCityDistricts(new List<Guid> { new Guid() });

            result.Should().ThrowExactly<RoomForRentAnnouncementPreferenceCityDistrictsNullException>()
                .WithMessage("CityDistricts argument is required.");
        }

        [Fact]
        public void ChangeCityDistricts_Should_Throw_RoomForRentAnnouncementPreferenceCityDistrictsDuplicateValuesException_When_CityDistricts_Contains_Duplicates()
        {
            var roomForRentAnnouncementPreference = RoomForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomType(RoomTypeEnumeration.Single)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var cityDistrict = Guid.NewGuid();

            Action result = () => roomForRentAnnouncementPreference.ChangeCityDistricts(new List<Guid> { cityDistrict, cityDistrict });

            result.Should().ThrowExactly<RoomForRentAnnouncementPreferenceCityDistrictsDuplicateValuesException>()
                .WithMessage("CityDistricts argument contains duplicate values.");
        }
    }
}