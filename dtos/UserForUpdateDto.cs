using System.Collections.Generic;

namespace devbook.api.dtos
{
    public class UserForUpdateDto
    {
        public string Status { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string GithubLink { get; set; }
        public string Interest { get; set; }

    }
}