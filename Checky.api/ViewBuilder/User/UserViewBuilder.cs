using System;

namespace Checky.api.ViewBuilder.User
{
    public class UserViewBuilder : IUserViewBuilder
    {
        public UserViewBuilder()
        {
        }

        public View.User Build(Model.User user)
        {
            return new View.User
            {
                Email = user.Email,
                Pin = user.Pin,
                Gender = user.Gender,
                Birthdate = user.Birthdate.ToString(),
                CreatedOn = user.CreatedOn.ToString(),
                UpdatedOn = user.UpdatedOn.ToString()
            };
        }
    }
}
