using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.DTO
{
    public interface IUpdatableDto
    {
        int Id { get; set; }
        bool IsUpdateOperation { get; set; }
    }
    public interface IGuidUpdatableDto
    {
        Guid Id { get; set; }
        bool IsUpdateOperation { get; set; }
    }
}
