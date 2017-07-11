using System;

namespace PersistentMailbox.Department
{
    public class Department
    {
        public string Name { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();
    }
}