using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SacrMeetPlanner.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SacrMeetPlanner.Pages.Meetings
{
    public class IndexModel : PageModel
    {
        private readonly SacrMeetPlanner.Models.SacrMeetPlannerContext _context;

        public IndexModel(SacrMeetPlanner.Models.SacrMeetPlannerContext context)
        {
            _context = context;
        }

        public IList<Meeting> Meeting { get;set; }

        // Eddie Pincay
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        // Requires using Microsoft.AspNetCore.Mvc.Rendering;
        public SelectList Presidings { get; set; }
        [BindProperty(SupportsGet = true)]
        public string MeetingPresiding { get; set; }

        public async Task OnGetAsync()
        {
            // Use LINQ to get list of Presiding.
            IQueryable<string> presidQuery = from m in _context.Meeting
                                             orderby m.Subject
                                             select m.Subject;

            var meetings = from m in _context.Meeting
                           select m;
            if (!String.IsNullOrEmpty(SearchString))
            {
                meetings = meetings.Where(s => s.Subject.Contains(SearchString));
            }

            if (!String.IsNullOrEmpty(MeetingPresiding))
            {
                meetings = meetings.Where(x => x.Subject == MeetingPresiding);
            }
            Presidings = new SelectList(await presidQuery.Distinct().ToListAsync());

            Meeting = await meetings.ToListAsync();
        }
    }
}
