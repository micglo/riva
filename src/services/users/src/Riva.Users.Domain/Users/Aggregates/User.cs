using System;
using System.Collections.Generic;
using System.Linq;
using Riva.BuildingBlocks.Domain;
using Riva.Users.Domain.Users.Builders;
using Riva.Users.Domain.Users.Entities;
using Riva.Users.Domain.Users.Enumerations;
using Riva.Users.Domain.Users.Events;
using Riva.Users.Domain.Users.ValueObjects.AggregateValueObjects;

namespace Riva.Users.Domain.Users.Aggregates
{
    public class User : AggregateBase
    {
        private List<RoomForRentAnnouncementPreference> _roomForRentAnnouncementPreferences;
        private List<FlatForRentAnnouncementPreference> _flatForRentAnnouncementPreferences;

        public string Email { get; private set; }
        public string Picture { get; private set; }
        public bool ServiceActive { get; private set; }
        public int AnnouncementPreferenceLimit { get; private set; }
        public AnnouncementSendingFrequencyEnumeration AnnouncementSendingFrequency { get; private set; }
        public IReadOnlyCollection<RoomForRentAnnouncementPreference> RoomForRentAnnouncementPreferences => _roomForRentAnnouncementPreferences.AsReadOnly();
        public IReadOnlyCollection<FlatForRentAnnouncementPreference> FlatForRentAnnouncementPreferences => _flatForRentAnnouncementPreferences.AsReadOnly();

        private User(UserBuilder builder) : base(builder.Id)
        {
            Email = builder.Email;
            Picture = builder.Picture;
            ServiceActive = builder.ServiceActive;
            AnnouncementPreferenceLimit = builder.AnnouncementPreferenceLimit;
            AnnouncementSendingFrequency = builder.AnnouncementSendingFrequency;
            _roomForRentAnnouncementPreferences = builder.RoomForRentAnnouncementPreferences;
            _flatForRentAnnouncementPreferences = builder.FlatForRentAnnouncementPreferences;
        }

        public static IUserIdSetter Builder()
        {
            return new UserBuilder();
        }

        public void AddCreatedEvent(Guid correlationId)
        {
            AddEvent(new UserCreatedDomainEvent(Id, correlationId, Email, Picture, ServiceActive,
                AnnouncementPreferenceLimit, AnnouncementSendingFrequency));
        }

        public void ChangeAnnouncementPreferenceLimit(int announcementPreferenceLimit, Guid correlationId)
        {
            AnnouncementPreferenceLimit = new UserAnnouncementPreferenceLimit(announcementPreferenceLimit);
            AddEvent(new UserAnnouncementPreferenceLimitChangedDomainEvent(Id, correlationId, AnnouncementPreferenceLimit));
        }

        public void ChangeAnnouncementSendingFrequency(AnnouncementSendingFrequencyEnumeration announcementSendingFrequency, Guid correlationId)
        {
            AnnouncementSendingFrequency = new UserAnnouncementSendingFrequency(announcementSendingFrequency);
            AddEvent(new UserAnnouncementSendingFrequencyChangedDomainEvent(Id, correlationId, AnnouncementSendingFrequency));
        }

        public void ChangeServiceActive(bool serviceActive, Guid correlationId)
        {
            ServiceActive = serviceActive;
            AddEvent(new UserServiceActiveChangedDomainEvent(Id, correlationId, ServiceActive));
        }

        public void ChangePicture(string picture, Guid correlationId)
        {
            Picture = picture;
            AddEvent(new UserPictureChangedDomainEvent(Id, correlationId, Picture));
        }

        public void AddFlatForRentAnnouncementPreference(FlatForRentAnnouncementPreference flatForRentAnnouncementPreference, Guid correlationId)
        {
            _flatForRentAnnouncementPreferences.Add(flatForRentAnnouncementPreference);
            _flatForRentAnnouncementPreferences = new UserFlatForRentAnnouncementPreferences(_flatForRentAnnouncementPreferences, _roomForRentAnnouncementPreferences.Count,
                AnnouncementPreferenceLimit);
            AddEvent(new UserFlatForRentAnnouncementPreferenceAddedDomainEvent(Id, correlationId, flatForRentAnnouncementPreference));
        }

        public void AddFlatForRentAnnouncementPreferenceChangedEvent(FlatForRentAnnouncementPreference flatForRentAnnouncementPreference, Guid correlationId)
        {
            AddEvent(new UserFlatForRentAnnouncementPreferenceChangedDomainEvent(Id, correlationId, flatForRentAnnouncementPreference));
        }

        public void DeleteFlatForRentAnnouncementPreference(FlatForRentAnnouncementPreference flatForRentAnnouncementPreference, Guid correlationId)
        {
            _flatForRentAnnouncementPreferences.Remove(flatForRentAnnouncementPreference);
            AddEvent(new UserFlatForRentAnnouncementPreferenceDeletedDomainEvent(Id, correlationId, flatForRentAnnouncementPreference));
        }

        public void AddRoomForRentAnnouncementPreference(RoomForRentAnnouncementPreference roomForRentAnnouncementPreference, Guid correlationId)
        {
            _roomForRentAnnouncementPreferences.Add(roomForRentAnnouncementPreference);
            _roomForRentAnnouncementPreferences = new UserRoomForRentAnnouncementPreferences(_roomForRentAnnouncementPreferences, _flatForRentAnnouncementPreferences.Count,
                AnnouncementPreferenceLimit);
            AddEvent(new UserRoomForRentAnnouncementPreferenceAddedDomainEvent(Id, correlationId, roomForRentAnnouncementPreference));
        }

        public void AddRoomForRentAnnouncementPreferenceChangedEvent(RoomForRentAnnouncementPreference roomForRentAnnouncementPreference, Guid correlationId)
        {
            AddEvent(new UserRoomForRentAnnouncementPreferenceChangedDomainEvent(Id, correlationId, roomForRentAnnouncementPreference));
        }

        public void DeleteRoomForRentAnnouncementPreference(RoomForRentAnnouncementPreference roomForRentAnnouncementPreference, Guid correlationId)
        {
            _roomForRentAnnouncementPreferences.Remove(roomForRentAnnouncementPreference);
            AddEvent(new UserRoomForRentAnnouncementPreferenceDeletedDomainEvent(Id, correlationId, roomForRentAnnouncementPreference));
        }

        public void AddDeletedEvent(Guid correlationId)
        {
            AddEvent(new UserDeletedDomainEvent(Id, correlationId));
        }

        public override void ApplyEvents()
        {
            base.ApplyEvents();

            foreach (var domainEvent in Events)
            {
                switch (domainEvent)
                {
                    case UserCreatedDomainEvent userCreatedDomainEvent:
                    {
                        Email = userCreatedDomainEvent.Email;
                        Picture = userCreatedDomainEvent.Picture;
                        ServiceActive = userCreatedDomainEvent.ServiceActive;
                        AnnouncementPreferenceLimit = userCreatedDomainEvent.AnnouncementPreferenceLimit;
                        AnnouncementSendingFrequency = userCreatedDomainEvent.AnnouncementSendingFrequency;
                        break;
                    }
                    case UserAnnouncementPreferenceLimitChangedDomainEvent userAnnouncementPreferenceLimitChangedDomainEvent:
                    {
                        AnnouncementPreferenceLimit = userAnnouncementPreferenceLimitChangedDomainEvent.AnnouncementPreferenceLimit;
                        break;
                    }
                    case UserAnnouncementSendingFrequencyChangedDomainEvent userAnnouncementSendingFrequencyChangedDomainEvent:
                    {
                        AnnouncementSendingFrequency = userAnnouncementSendingFrequencyChangedDomainEvent.AnnouncementSendingFrequency;
                        break;
                    }
                    case UserServiceActiveChangedDomainEvent userServiceActiveChangedDomainEvent:
                    {
                        ServiceActive = userServiceActiveChangedDomainEvent.ServiceActive;
                        break;
                    }
                    case UserPictureChangedDomainEvent userPictureChangedDomainEvent:
                    {
                        Picture = userPictureChangedDomainEvent.Picture;
                        break;
                    }
                    case UserFlatForRentAnnouncementPreferenceAddedDomainEvent userFlatForRentAnnouncementPreferenceAddedDomainEvent:
                    {
                        _flatForRentAnnouncementPreferences.Add(userFlatForRentAnnouncementPreferenceAddedDomainEvent.FlatForRentAnnouncementPreference);
                        break;
                    }
                    case UserFlatForRentAnnouncementPreferenceChangedDomainEvent userFlatForRentAnnouncementPreferenceChangedDomainEvent:
                    {
                        var flatForRentAnnouncementPreference = _flatForRentAnnouncementPreferences.Single(x =>
                            x.Id == userFlatForRentAnnouncementPreferenceChangedDomainEvent.FlatForRentAnnouncementPreference.Id);
                        flatForRentAnnouncementPreference.ChangeCityId(userFlatForRentAnnouncementPreferenceChangedDomainEvent.FlatForRentAnnouncementPreference.CityId);
                        flatForRentAnnouncementPreference.ChangePriceMin(userFlatForRentAnnouncementPreferenceChangedDomainEvent.FlatForRentAnnouncementPreference.PriceMin);
                        flatForRentAnnouncementPreference.ChangePriceMax(userFlatForRentAnnouncementPreferenceChangedDomainEvent.FlatForRentAnnouncementPreference.PriceMax);
                        flatForRentAnnouncementPreference.ChangeRoomNumbersMin(userFlatForRentAnnouncementPreferenceChangedDomainEvent.FlatForRentAnnouncementPreference.RoomNumbersMin);
                        flatForRentAnnouncementPreference.ChangeRoomNumbersMax(userFlatForRentAnnouncementPreferenceChangedDomainEvent.FlatForRentAnnouncementPreference.RoomNumbersMax);
                        flatForRentAnnouncementPreference.ChangeCityDistricts(userFlatForRentAnnouncementPreferenceChangedDomainEvent.FlatForRentAnnouncementPreference.CityDistricts);
                        break;
                    }
                    case UserFlatForRentAnnouncementPreferenceDeletedDomainEvent userFlatForRentAnnouncementPreferenceDeletedDomainEvent:
                    {
                        _flatForRentAnnouncementPreferences.Remove(userFlatForRentAnnouncementPreferenceDeletedDomainEvent.FlatForRentAnnouncementPreference);
                        break;
                    }
                    case UserRoomForRentAnnouncementPreferenceAddedDomainEvent userRoomForRentAnnouncementPreferenceAddedDomainEvent:
                    {
                        _roomForRentAnnouncementPreferences.Add(userRoomForRentAnnouncementPreferenceAddedDomainEvent.RoomForRentAnnouncementPreference);
                        break;
                    }
                    case UserRoomForRentAnnouncementPreferenceChangedDomainEvent userRoomForRentAnnouncementPreferenceChangedDomainEvent:
                    {
                        var roomForRentAnnouncementPreference = _roomForRentAnnouncementPreferences.Single(x =>
                            x.Id == userRoomForRentAnnouncementPreferenceChangedDomainEvent.RoomForRentAnnouncementPreference.Id);
                        roomForRentAnnouncementPreference.ChangeCityId(userRoomForRentAnnouncementPreferenceChangedDomainEvent.RoomForRentAnnouncementPreference.CityId);
                        roomForRentAnnouncementPreference.ChangePriceMin(userRoomForRentAnnouncementPreferenceChangedDomainEvent.RoomForRentAnnouncementPreference.PriceMin);
                        roomForRentAnnouncementPreference.ChangePriceMax(userRoomForRentAnnouncementPreferenceChangedDomainEvent.RoomForRentAnnouncementPreference.PriceMax);
                        roomForRentAnnouncementPreference.ChangeRoomType(userRoomForRentAnnouncementPreferenceChangedDomainEvent.RoomForRentAnnouncementPreference.RoomType);
                        roomForRentAnnouncementPreference.ChangeCityDistricts(userRoomForRentAnnouncementPreferenceChangedDomainEvent.RoomForRentAnnouncementPreference.CityDistricts);
                        break;
                    }
                    case UserRoomForRentAnnouncementPreferenceDeletedDomainEvent userRoomForRentAnnouncementPreferenceDeletedDomainEvent:
                    {
                        _roomForRentAnnouncementPreferences.Remove(userRoomForRentAnnouncementPreferenceDeletedDomainEvent.RoomForRentAnnouncementPreference);
                        break;
                    }
                }
            }
        }

        private class UserBuilder : IUserIdSetter, IUserEmailSetter, IUserServiceActiveSetter, IUserAnnouncementPreferenceLimitSetter, IUserAnnouncementSendingFrequencySetter, IUserBuilder
        {
            public Guid Id { get; private set; }
            public string Email { get; private set; }
            public string Picture { get; private set; }
            public bool ServiceActive { get; private set; }
            public int AnnouncementPreferenceLimit { get; private set; }
            public AnnouncementSendingFrequencyEnumeration AnnouncementSendingFrequency { get; private set; }
            public List<RoomForRentAnnouncementPreference> RoomForRentAnnouncementPreferences { get; private set; } = new List<RoomForRentAnnouncementPreference>();
            public List<FlatForRentAnnouncementPreference> FlatForRentAnnouncementPreferences { get; private set; } = new List<FlatForRentAnnouncementPreference>();

            public IUserEmailSetter SetId(Guid id)
            {
                Id = id;
                return this;
            }

            public IUserServiceActiveSetter SetEmail(string email)
            {
                Email = new UserEmail(email);
                return this;
            }

            public IUserAnnouncementPreferenceLimitSetter SetServiceActive(bool serviceActive)
            {
                ServiceActive = serviceActive;
                return this;
            }

            public IUserAnnouncementSendingFrequencySetter SetAnnouncementPreferenceLimit(int announcementPreferenceLimit)
            {
                AnnouncementPreferenceLimit = new UserAnnouncementPreferenceLimit(announcementPreferenceLimit);
                return this;
            }

            public IUserBuilder SetAnnouncementSendingFrequency(AnnouncementSendingFrequencyEnumeration announcementSendingFrequency)
            {
                AnnouncementSendingFrequency = new UserAnnouncementSendingFrequency(announcementSendingFrequency);
                return this;
            }

            public IUserBuilder SetPicture(string picture)
            {
                Picture = picture;
                return this;
            }

            public IUserBuilder SetRoomForRentAnnouncementPreferences(IEnumerable<RoomForRentAnnouncementPreference> roomForRentAnnouncementPreferences)
            {
                RoomForRentAnnouncementPreferences = new UserRoomForRentAnnouncementPreferences(roomForRentAnnouncementPreferences, FlatForRentAnnouncementPreferences.Count, 
                    AnnouncementPreferenceLimit);
                return this;
            }

            public IUserBuilder SetFlatForRentAnnouncementPreferences(IEnumerable<FlatForRentAnnouncementPreference> flatForRentAnnouncementPreferences)
            {
                FlatForRentAnnouncementPreferences = new UserFlatForRentAnnouncementPreferences(flatForRentAnnouncementPreferences, RoomForRentAnnouncementPreferences.Count,
                    AnnouncementPreferenceLimit);
                return this;
            }

            public User Build()
            {
                return new User(this);
            }
        }
    }
}