using course_tracker.Models;
using course_tracker.Models.exceptions;
using course_tracker.Models.extensions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace course_tracker.Services
{
    public class AssessmentRepository
    {
        private readonly SQLiteAsyncConnection _sqlConn;
        public AssessmentRepository(SQLiteAsyncConnection sqlConn)
        {
            _sqlConn = sqlConn;
        }

        public async Task<List<Assessment>> GetAssessmentsAsync(int courseId)
        {
            return await _sqlConn.Table<Assessment>().Where(c => c.CourseId == courseId).ToListAsync();
        }

        public async Task<Assessment> GetAssessmentByIdAsync(int id)
        {
            return await _sqlConn.Table<Assessment>().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Assessment> AddAssessmentAsync(Assessment assessment)
        {
            var course = await _sqlConn.Table<Course>().FirstOrDefaultAsync(c => c.Id == assessment.CourseId);
            if (course == null) throw new PublicException("Assessment must be tied to Course.");

            ValidateAssessment(course, assessment);

            var existingAssessment = await _sqlConn.Table<Assessment>().FirstOrDefaultAsync(a => a.CourseId == assessment.CourseId && a.Type == assessment.Type);
            if (existingAssessment == null)
            {
                switch (assessment.Type)
                {
                    case AssessmentType.Objective:
                        throw new PublicException("Course already has an Objective Assessment.");
                    case AssessmentType.Performance:
                        throw new PublicException("Course already has an Performance Assessment.");
                }
            }

            var id = await _sqlConn.InsertAsync(course);
            return await GetAssessmentByIdAsync(id);
        }

        public async Task<Assessment> UpdateCourseAsync(Assessment assessment)
        {
            var course = await _sqlConn.Table<Course>().FirstOrDefaultAsync(c => c.Id == assessment.CourseId);
            if (course == null) throw new PublicException("Assessment must be tied to Course.");

            ValidateAssessment(course, assessment);

            var existingAssessment = await _sqlConn.Table<Assessment>().FirstOrDefaultAsync(a => a.CourseId == assessment.CourseId && a.Type == assessment.Type);
            if (existingAssessment == null)
            {
                switch (assessment.Type)
                {
                    case AssessmentType.Objective:
                        throw new PublicException("Course already has an Objective Assessment.");
                    case AssessmentType.Performance:
                        throw new PublicException("Course already has an Performance Assessment.");
                }
            }

            await _sqlConn.UpdateAsync(course);
            return assessment;
        }

        public async Task<bool> RemoveAssessmentAsync(int id)
        {
            var count = await _sqlConn.DeleteAsync(new Assessment { Id = id });
            return count > 0;
        }

        private void ValidateAssessment(Course course, Assessment assessment)
        {
            if (assessment.Title.IsNull()) throw new PublicException("Assement must have a name.");

            if (assessment.Start >= assessment.End) throw new PublicException("Assements start date must be before the end date.");

            if (course.Start < assessment.Start) throw new PublicException("Assement cannot begin before course start date.");

            if (course.End > assessment.End) throw new PublicException("Assement cannot end after course end date.");
        }
    }
}
