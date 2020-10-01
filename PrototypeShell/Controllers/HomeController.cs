using Microsoft.AspNetCore.Mvc;
using PrototypeShell.Models;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace PrototypeShell.Controllers
{
    public class HomeController : Controller
    {
        private readonly CommandContext _db;
        private static int _latestFetchedRecord = 0;
        private static int _latestRecordId = 0;

        public HomeController(CommandContext context)
        {
            _db = context;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ContentResult> ProcessComm(string command)
        {
            Command comObj = ParseInput(command);
            _db.Commands.Add(comObj);
            await _db.SaveChangesAsync();
            _latestRecordId = comObj.CommandId;
            _latestFetchedRecord = _latestRecordId;
            return Content(ShellRun(RunInfo.CurrentPath, comObj.Exec, comObj.Args));
        }


        public ContentResult StatusInit()
        {
            return Content(
                $"{RunInfo.Username}@{RunInfo.Hostname} {RunInfo.CurrentPath} |>&nbsp");
        }

        public ContentResult HistoryUp()
        {
            var next = FetchInHistory();

            if (_latestFetchedRecord != _latestRecordId)
            {
                _latestFetchedRecord += 1;
            }

            return Content(next.SingleOrDefault().CommandReq);
        }

        public ContentResult HistoryDown()
        {

            var prev = FetchInHistory();

            if (_latestFetchedRecord != 0)
            {
                _latestFetchedRecord -= 1;
            }

            return Content(prev.SingleOrDefault().CommandReq);
        }

        private IQueryable<Command> FetchInHistory()
        {
            var cmd = (from c in _db.Commands
                       where c.CommandId == _latestFetchedRecord
                       select new Command
                       {
                           CommandId = c.CommandId,
                           CommandReq = c.CommandReq,
                           Args = c.Args,
                           Exec = c.Exec,
                           Time = c.Time
                       });
            return cmd;
        }

        private Command ParseInput(string command)
        {
            string[] splitted = command.Split();

            Command comObj = new Command
            {
                CommandReq = command,
                Exec = splitted[0],
                Args = String.Join(' ', splitted[1..^0]),
                Time = DateTime.Now
            };

            if (comObj.Exec == "cd")
            {
                RunInfo.CurrentPath = comObj.Args;
            }
            return comObj;
        }

        private string ShellRun(string wd, string exec, string args)
        {
            WinShellRun wsr = new WinShellRun();
            wsr.Call(wd, exec, args);
            return StyleWrapper(wsr);
        }

        private string StyleWrapper(WinShellRun wsr)
        {
            string top = $"<em>&lt {wsr.Command}</em><br>";
            string bottom = $"<em>&gt = {wsr.ExitCode}</em><br><br>";
            string wrapped;
            if (wsr.ExitCode != 0)
            {
                top = "<strong>" + top;
                wrapped = $"<p>{wsr.StandardErr}</p>";
                bottom += "</strong>";
            } else
            {
                wrapped = $"<p>{wsr.StandardOut}</p>";
            }
            return top + wrapped + bottom;
        }   
    }
}
