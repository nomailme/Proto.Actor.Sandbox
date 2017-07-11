using System.Collections.Generic;

namespace PersistentMailbox.Department.Messages
{
    public class GetDepartmentsResponse
    {
        public IEnumerable<DepartmentInfo> DepartmentInfos { get; set; }
    }
}