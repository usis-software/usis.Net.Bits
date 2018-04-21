using System;
using System.Diagnostics;

namespace usis.Server.FieldService
{
    public class SnapIn : Framework.SnapIn
    {
        protected override void OnConnected(EventArgs e)
        {
            base.OnConnected(e);

            using (var db = new DbContext())
            {
                {
                    var assignment = Assignment.NewAssignment();
                    assignment.Subject = DateTime.Now.ToString();
                    db.Assignments.Add(assignment);
                    db.SaveChanges();
                }
                foreach (var assignment in db.Assignments)
                {
                    Debug.WriteLine(assignment.Subject);
                }
            }
        }
    }
}
