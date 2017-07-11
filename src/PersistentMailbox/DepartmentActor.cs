using System.Threading.Tasks;
using PersistentMailbox.Department;
using PersistentMailbox.Department.Messages;
using Proto;
using Serilog;

namespace PersistentMailbox
{
    public class DepartmentActor : IActor
    {
        private readonly ILogger _logger;
        private string _name;

        public DepartmentActor(ILogger logger)
        {
            _logger = logger;
        }

        public Task ReceiveAsync(IContext context)
        {
            switch (context.Message)
            {
                case CreateDepartmentMessage message:
                    var department = new Department.Department {Name = message.Name};
                    _name = message.Name;
                    _logger.Information("Created department @{department}", department);
                    break;
                case GetDepartmentsRequest request:
                    context.Respond(new DepartmentInfo{Name = _name});
                    break;
                    
            }
            return Actor.Done;
        }
    }
}