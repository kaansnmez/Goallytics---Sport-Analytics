using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Application.Dtos
{
    public class CheckUserResponseDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Role { get; set; }
        public bool IsExist { get; set; }
    }
}
