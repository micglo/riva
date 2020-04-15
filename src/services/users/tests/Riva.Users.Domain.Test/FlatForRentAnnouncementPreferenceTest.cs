using System;
using System.Collections.Generic;
using FluentAssertions;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Domain.Users.Exceptions.EntityExceptions.FlatForRentAnnouncementPreferenceExceptions;
using Xunit;

namespace Riva.Users.Domain.Test
{
    public class FlatForRentAnnouncementPreferenceTest
    {
        [Fact]
        public void Should_Create_FlatForRentAnnouncementPreference()
        {
            var flatForRentAnnouncementPreferenceId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var cityDistricts = new List<Guid> { Guid.NewGuid() };
            const decimal priceMin = 0;
            const decimal priceMax = 2000;
            const int roomNumberMin = 1;
            const int roomNumberMax = 2;
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(flatForRentAnnouncementPreferenceId)
                .SetCityId(cityId)
                .SetPriceMin(priceMin)
                .SetPriceMax(priceMax)
                .SetRoomNumbersMin(roomNumberMin)
                .SetRoomNumbersMax(roomNumberMax)
                .SetCityDistricts(cityDistricts)
                .Build();

            flatForRentAnnouncementPreference.Id.Should().Be(flatForRentAnnouncementPreferenceId);
            flatForRentAnnouncementPreference.CityId.Should().Be(cityId);
            flatForRentAnnouncementPreference.PriceMin.Should().Be(priceMin);
            flatForRentAnnouncementPreference.PriceMax.Should().Be(priceMax);
            flatForRentAnnouncementPreference.RoomNumbersMin.Should().Be(roomNumberMin);
            flatForRentAnnouncementPreference.RoomNumbersMax.Should().Be(roomNumberMax);
            flatForRentAnnouncementPreference.CityDistricts.Should().BeEquivalentTo(cityDistricts);
        }

        [Fact]
        public void Should_Create_FlatForRentAnnouncementPreference_With_Null_Values()
        {
            var flatForRentAnnouncementPreferenceId = Guid.NewGuid();
            var cityId = Guid.NewGuid();
            var cityDistricts = new List<Guid> { Guid.NewGuid() };
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(flatForRentAnnouncementPreferenceId)
                .SetCityId(cityId)
                .SetPriceMin(null)
                .SetPriceMax(null)
                .SetRoomNumbersMin(null)
                .SetRoomNumbersMax(null)
                .SetCityDistricts(cityDistricts)
                .Build();

            flatForRentAnnouncementPreference.Id.Should().Be(flatForRentAnnouncementPreferenceId);
            flatForRentAnnouncementPreference.CityId.Should().Be(cityId);
            flatForRentAnnouncementPreference.PriceMin.Should().BeNull();
            flatForRentAnnouncementPreference.PriceMax.Should().BeNull();
            flatForRentAnnouncementPreference.RoomNumbersMin.Should().BeNull();
            flatForRentAnnouncementPreference.RoomNumbersMax.Should().BeNull();
            flatForRentAnnouncementPreference.CityDistricts.Should().BeEquivalentTo(cityDistricts);
        }

        [Fact]
        public void Should_Throw_FlatForRentAnnouncementPreferenceIdNullException_When_Id_Is_Empty_Guid()
        {
            Action result = () =>
            {
                var unused = FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.Empty)
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(new List<Guid> {Guid.NewGuid()})
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferenceIdNullException>()
                .WithMessage("Id argument is required.");
        }

        [Fact]
        public void Should_Throw_FlatForRentAnnouncementPreferenceIdNullException_When_Id_Is_New_Guid()
        {
            Action result = () =>
            {
                var unused = FlatForRentAnnouncementPreference.Builder()
                    .SetId(new Guid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferenceIdNullException>()
                .WithMessage("Id argument is required.");
        }

        [Fact]
        public void Should_Throw_FlatForRentAnnouncementPreferenceCityIdNullException_When_CityId_Is_Empty_Guid()
        {
            Action result = () =>
            {
                var unused = FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.Empty)
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferenceCityIdNullException>()
                .WithMessage("CityId argument is required.");
        }

        [Fact]
        public void Should_Throw_FlatForRentAnnouncementPreferenceCityIdNullException_When_CityId_Is_New_Guid()
        {
            Action result = () =>
            {
                var unused = FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(new Guid())
                    .SetPriceMin(0)
                    .SetPriceMax(2000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferenceCityIdNullException>()
                .WithMessage("CityId argument is required.");
        }

        [Fact]
        public void Should_Throw_FlatForRentAnnouncementPreferencePriceMinMinValueException_When_PriceMin_Is_Negative_Value()
        {
            Action result = () =>
            {
                var unused = FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(-1)
                    .SetPriceMax(2000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferencePriceMinMinValueException>()
                .WithMessage("PriceMin argument min value is 0.");
        }

        [Fact]
        public void Should_Throw_FlatForRentAnnouncementPreferencePriceMaxMinValueException_When_PriceMax_Is_Negative_Value()
        {
            Action result = () =>
            {
                var unused = FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(-1)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferencePriceMaxMinValueException>()
                .WithMessage("PriceMax argument min value is 0.");
        }

        [Fact]
        public void Should_Throw_FlatForRentAnnouncementPreferencePriceMaxLowerThanPriceMinException_When_PriceMax_Is_Lower_Than_PriceMin()
        {
            Action result = () =>
            {
                var unused = FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(1000)
                    .SetPriceMax(0)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferencePriceMaxLowerThanPriceMinException>()
                .WithMessage("PriceMax argument cannot be lower than PriceMin.");
        }

        [Fact]
        public void Should_Throw_FlatForRentAnnouncementPreferenceRoomNumbersMinMinValueException_When_RoomNumbersMin_Is_Negative_Value()
        {
            Action result = () =>
            {
                var unused = FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomNumbersMin(0)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferenceRoomNumbersMinMinValueException>()
                .WithMessage("RoomNumbersMin argument min value is 1.");
        }

        [Fact]
        public void Should_Throw_FlatForRentAnnouncementPreferenceRoomNumbersMaxMinValueException_When_RoomNumbersMax_Is_Negative_Value()
        {
            Action result = () =>
            {
                var unused = FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(0)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferenceRoomNumbersMaxMinValueException>()
                .WithMessage("RoomNumbersMax argument min value is 1.");
        }

        [Fact]
        public void Should_Throw_FlatForRentAnnouncementPreferenceRoomNumbersMaxLowerThanRoomNumbersMinException_When_RoomNumbersMax_Is_Lower_Than_RoomNumbersMin()
        {
            Action result = () =>
            {
                var unused = FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomNumbersMin(2)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferenceRoomNumbersMaxLowerThanRoomNumbersMinException>()
                .WithMessage("RoomNumbersMax argument cannot be lower than RoomNumbersMin.");
        }

        [Fact]
        public void Should_Throw_FlatForRentAnnouncementPreferenceCityDistrictsNullException_When_CityDistricts_Is_Null()
        {
            Action result = () =>
            {
                var unused = FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(null)
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferenceCityDistrictsNullException>()
                .WithMessage("CityDistricts argument is required.");
        }

        [Fact]
        public void Should_Throw_FlatForRentAnnouncementPreferenceCityDistrictsNullException_When_CityDistricts_Contains_Empty_Guid()
        {
            Action result = () =>
            {
                var unused = FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(new List<Guid> {Guid.Empty})
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferenceCityDistrictsNullException>()
                .WithMessage("CityDistricts argument is required.");
        }

        [Fact]
        public void Should_Throw_FlatForRentAnnouncementPreferenceCityDistrictsNullException_When_CityDistricts_Contains_New_Guid()
        {
            Action result = () =>
            {
                var unused = FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(new List<Guid> { new Guid() })
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferenceCityDistrictsNullException>()
                .WithMessage("CityDistricts argument is required.");
        }

        [Fact]
        public void Should_Throw_FlatForRentAnnouncementPreferenceCityDistrictsDuplicateValuesException_When_CityDistricts_Contains_Duplicates()
        {
            var cityDistrict = Guid.NewGuid();
            Action result = () =>
            {
                var unused = FlatForRentAnnouncementPreference.Builder()
                    .SetId(Guid.NewGuid())
                    .SetCityId(Guid.NewGuid())
                    .SetPriceMin(0)
                    .SetPriceMax(1000)
                    .SetRoomNumbersMin(1)
                    .SetRoomNumbersMax(1)
                    .SetCityDistricts(new List<Guid> { cityDistrict, cityDistrict })
                    .Build();
            };

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferenceCityDistrictsDuplicateValuesException>()
                .WithMessage("CityDistricts argument contains duplicate values.");
        }

        [Fact]
        public void ChangeCityId_Should_Change_CityId()
        {
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var newCityId = Guid.NewGuid();

            flatForRentAnnouncementPreference.ChangeCityId(newCityId);

            flatForRentAnnouncementPreference.CityId.Should().Be(newCityId);
        }

        [Fact]
        public void ChangeCityId_Should_Throw_FlatForRentAnnouncementPreferenceCityIdNullException_When_CityId_Is_Empty_Guid()
        {
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => flatForRentAnnouncementPreference.ChangeCityId(Guid.Empty);

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferenceCityIdNullException>()
                .WithMessage("CityId argument is required.");
        }

        [Fact]
        public void ChangeCityId_Should_Throw_FlatForRentAnnouncementPreferenceCityIdNullException_When_CityId_Is_New_Guid()
        {
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => flatForRentAnnouncementPreference.ChangeCityId(new Guid());

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferenceCityIdNullException>()
                .WithMessage("CityId argument is required.");
        }

        [Fact]
        public void ChangePriceMin_Should_Change_PriceMin()
        {
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            flatForRentAnnouncementPreference.ChangePriceMin(null);

            flatForRentAnnouncementPreference.PriceMin.Should().BeNull();
        }

        [Fact]
        public void ChangePriceMin_Should_Throw_FlatForRentAnnouncementPreferencePriceMinMinValueException_When_PriceMin_Is_Negative_Value()
        {
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => flatForRentAnnouncementPreference.ChangePriceMin(-1);

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferencePriceMinMinValueException>()
                .WithMessage("PriceMin argument min value is 0.");
        }

        [Fact]
        public void ChangePriceMax_Should_Change_PriceMax()
        {
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            flatForRentAnnouncementPreference.ChangePriceMax(null);

            flatForRentAnnouncementPreference.PriceMax.Should().BeNull();
        }

        [Fact]
        public void ChangePriceMax_Should_Throw_FlatForRentAnnouncementPreferencePriceMaxMinValueException_When_PriceMax_Is_Negative_Value()
        {
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => flatForRentAnnouncementPreference.ChangePriceMax(-1);

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferencePriceMaxMinValueException>()
                .WithMessage("PriceMax argument min value is 0.");
        }

        [Fact]
        public void ChangePriceMax_Should_Throw_FlatForRentAnnouncementPreferencePriceMaxLowerThanPriceMinException_When_PriceMax_Is_Lower_Than_PriceMin()
        {
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(1000)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => flatForRentAnnouncementPreference.ChangePriceMax(0);

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferencePriceMaxLowerThanPriceMinException>()
                .WithMessage("PriceMax argument cannot be lower than PriceMin.");
        }

        [Fact]
        public void ChangeRoomNumbersMin_Should_Change_RoomNumbersMin()
        {
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            flatForRentAnnouncementPreference.ChangeRoomNumbersMin(null);

            flatForRentAnnouncementPreference.RoomNumbersMin.Should().BeNull();
        }

        [Fact]
        public void ChangeRoomNumbersMin_Should_Throw_FlatForRentAnnouncementPreferenceRoomNumbersMinMinValueException_When_RoomNumbersMin_Is_Negative_Value()
        {
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => flatForRentAnnouncementPreference.ChangeRoomNumbersMin(0);

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferenceRoomNumbersMinMinValueException>()
                .WithMessage("RoomNumbersMin argument min value is 1.");
        }

        [Fact]
        public void ChangeRoomNumbersMax_Should_Change_RoomNumbersMax()
        {
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            flatForRentAnnouncementPreference.ChangeRoomNumbersMax(null);

            flatForRentAnnouncementPreference.RoomNumbersMax.Should().BeNull();
        }

        [Fact]
        public void ChangeRoomNumbersMax_Should_Throw_FlatForRentAnnouncementPreferenceRoomNumbersMaxMinValueException_When_RoomNumbersMax_Is_Negative_Value()
        {
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => flatForRentAnnouncementPreference.ChangeRoomNumbersMax(0);

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferenceRoomNumbersMaxMinValueException>()
                .WithMessage("RoomNumbersMax argument min value is 1.");
        }

        [Fact]
        public void ChangeRoomNumbersMax_Should_Throw_FlatForRentAnnouncementPreferenceRoomNumbersMaxLowerThanRoomNumbersMinException_When_RoomNumbersMax_Is_Lower_Than_RoomNumbersMin()
        {
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(2)
                .SetRoomNumbersMax(2)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => flatForRentAnnouncementPreference.ChangeRoomNumbersMax(1);

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferenceRoomNumbersMaxLowerThanRoomNumbersMinException>()
                .WithMessage("RoomNumbersMax argument cannot be lower than RoomNumbersMin.");
        }

        [Fact]
        public void ChangeCityDistricts_Should_Change_CityDistricts()
        {
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var newCityDistricts = new List<Guid> { Guid.NewGuid() };

            flatForRentAnnouncementPreference.ChangeCityDistricts(newCityDistricts);

            flatForRentAnnouncementPreference.CityDistricts.Should().BeEquivalentTo(newCityDistricts);
        }

        [Fact]
        public void ChangeCityDistricts_Should_Throw_FlatForRentAnnouncementPreferenceCityDistrictsNullException_When_CityDistricts_Is_Null()
        {
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => flatForRentAnnouncementPreference.ChangeCityDistricts(null);

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferenceCityDistrictsNullException>()
                .WithMessage("CityDistricts argument is required.");
        }

        [Fact]
        public void ChangeCityDistricts_Should_Throw_FlatForRentAnnouncementPreferenceCityDistrictsNullException_When_CityDistricts_Contains_Empty_Guid()
        {
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => flatForRentAnnouncementPreference.ChangeCityDistricts(new List<Guid> { Guid.Empty });

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferenceCityDistrictsNullException>()
                .WithMessage("CityDistricts argument is required.");
        }

        [Fact]
        public void ChangeCityDistricts_Should_Throw_FlatForRentAnnouncementPreferenceCityDistrictsNullException_When_CityDistricts_Contains_New_Guid()
        {
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();

            Action result = () => flatForRentAnnouncementPreference.ChangeCityDistricts(new List<Guid> { new Guid() });

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferenceCityDistrictsNullException>()
                .WithMessage("CityDistricts argument is required.");
        }

        [Fact]
        public void ChangeCityDistricts_Should_Throw_FlatForRentAnnouncementPreferenceCityDistrictsDuplicateValuesException_When_CityDistricts_Contains_Duplicates()
        {
            var flatForRentAnnouncementPreference = FlatForRentAnnouncementPreference.Builder()
                .SetId(Guid.NewGuid())
                .SetCityId(Guid.NewGuid())
                .SetPriceMin(0)
                .SetPriceMax(2000)
                .SetRoomNumbersMin(1)
                .SetRoomNumbersMax(1)
                .SetCityDistricts(new List<Guid> { Guid.NewGuid() })
                .Build();
            var cityDistrict = Guid.NewGuid();

            Action result = () => flatForRentAnnouncementPreference.ChangeCityDistricts(new List<Guid> { cityDistrict, cityDistrict });

            result.Should().ThrowExactly<FlatForRentAnnouncementPreferenceCityDistrictsDuplicateValuesException>()
                .WithMessage("CityDistricts argument contains duplicate values.");
        }
    }
}