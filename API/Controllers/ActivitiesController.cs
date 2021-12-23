using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        private readonly DataContext _db;

        public ActivitiesController(DataContext db)
        {
            this._db = db;
        }


        [HttpGet]

        public async Task<ActionResult<List<Activity>>> GetActivities()
        {
            return await _db.Activities.ToListAsync();
        }

        [HttpPost]
        [Route("/Add")]

        public async Task<ActionResult> AddActivity(Activity act)
        {
              _db.Activities.Add(act);

             await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("update/{id}")]

        public async Task<ActionResult> UpdateActivity(Guid id, [FromBody]Activity activity)
        {

            CommonResponse<int> commonResponse = new CommonResponse<int>();

            if (activity != null && id != Guid.Empty)
            {
                activity.Id = id;
                _db.Activities.Update(activity);
                await _db.SaveChangesAsync();
                commonResponse.status = 1;
                commonResponse.message = "Activity Updated successfully.";
            }

            else
            {
                commonResponse.status = 0;
                commonResponse.message = "Something went wront, Please try again.";
            }

           

            return Ok(commonResponse);
        }


        [HttpDelete("delete/{id}")]

        public async Task<ActionResult> DeleteActivity(Guid id)
        {

            CommonResponse<int> commonResponse = new CommonResponse<int>();
            Activity activityToDelet = _db.Activities.Find(id);

            if (activityToDelet != null)
            {
                
                _db.Activities.Remove(activityToDelet);

                await _db.SaveChangesAsync();

                commonResponse.status = 1;
                commonResponse.message = "Activity deleted successfully.";

            }
            else
            {
                commonResponse.status = 0;
                commonResponse.message = "Something went wront, Please try again.";
            }

            return Ok(commonResponse);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetAct(Guid id)
        {
           return await _db.Activities.FindAsync(id);
        }


    }
}
