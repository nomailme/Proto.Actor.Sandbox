using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersistentMailbox.Department;
using PersistentMailbox.Department.Messages;
using Proto;
using Serilog;

namespace PersistentMailbox
{
    public class DepartmentManagerActor : IActor
    {
        private readonly Dictionary<Guid, PID> _departments = new Dictionary<Guid, PID>();
        private readonly Dictionary<string,Guid> _ids = new Dictionary<string, Guid>();
        private readonly ILogger _logger;

        public DepartmentManagerActor(ILogger logger)
        {
            _logger = logger;
        }

        public async Task ReceiveAsync(IContext context)
        {
            var msg = context.Message;

            switch (msg)
            {
                case Started message:
                    _logger.Information($"{nameof(DepartmentManagerActor)} started");
                    break;
                case GetDepartmentsRequest request:
                    var response = new GetDepartmentsResponse();
                    var infos = _departments.Values.Select(async x => await x.RequestAsync<DepartmentInfo>(request));
                    response.DepartmentInfos = await Task.WhenAll(infos);
                    context.Respond(response);
                    break;
                case CreateDepartmentMessage message:
                    var department = GetDepartmentActor(message);
                    department.Tell(message);
                    break;
                case string message:
                    _logger.Information("Message @{message}", new {Message = message, Time = DateTime.Now});
                    break;
            }
        }

        private PID GetDepartmentActor(CreateDepartmentMessage message)
        {
            if (_ids.TryGetValue(message.Name, out var departmentActor))
            {
                return _departments[departmentActor];
            }
            var props = Actor.FromProducer(()=> new DepartmentActor(_logger));
            var actor = Actor.Spawn(props);

            var id = Guid.NewGuid();
            _departments.Add(id, actor);
            _ids.Add(message.Name,id);
            return actor;
        }
    }
}