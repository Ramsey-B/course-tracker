using course_tracker.models;
using course_tracker.models.exceptions;
using course_tracker.models.extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace course_tracker.service
{
    public class CourseService
    {
        private readonly TermService _termService;

        public CourseService(TermService termService)
        {
            _termService = termService;
        }

        public Course GetCourseById(Term term, string id)
        {
            return term?.Courses.Find(c => c.Id == id);
        }

        public List<Course> GetCourses(Term term)
        {
            return term.Courses ?? new List<Course>();
        }

        public Course AddCourse(Term term, Course newCourse)
        {
            var courses = GetCourses(term);

            newCourse.Id = Guid.NewGuid().ToString();

            if (courses.Count > 5) throw new PublicException("Cannot add course to term. Maximum number of 6 courses reached.");

            ValidateCourse(term, newCourse);

            courses.Add(newCourse);
            term.Courses = courses;

            _termService.UpdateTerm(term.Id, term);
            return newCourse;
        }

        public Course UpdateCourse(Term term, Course newCourse)
        {
            var courses = GetCourses(term);

            if (courses.Count > 5) throw new PublicException("Cannot add course to term. Maximum number of 6 courses reached.");

            ValidateCourse(term, newCourse);
            
            term.Courses = courses.Replace(c => c.Id == newCourse.Id, newCourse).ToList();

            _termService.UpdateTerm(term.Id, term);
            return newCourse;
        }

        public bool RemoveCourse(Term term, string id)
        {
            term.Courses.Remove(c => c.Id == id);
            return _termService.UpdateTerm(term.Id, term) != null;
        }

        public Course AddAssessment(Term term, Course course, Assessment assessment)
        {
            ValidateAssessment(course, assessment);
            if (assessment.Type == AssessmentType.Objective)
            {
                if (course.ObjectiveAssessment != null) throw new PublicException("Course already has an Objective Assessment.");
                course.ObjectiveAssessment = assessment;
            }
            else
            {
                if (course.PerformanceAssessment != null) throw new PublicException("Course already has an Performance Assessment.");
                course.PerformanceAssessment = assessment;
            }

            return UpdateCourse(term, course);
        }

        public Course UpdateAssessment(Term term, Course course, Assessment assessment)
        {
            ValidateAssessment(course, assessment);
            if (assessment.Type == AssessmentType.Objective)
            {
                if (course.ObjectiveAssessment != null) throw new PublicException("Course already has an Objective Assessment.");
                course.ObjectiveAssessment = assessment;
            }
            else
            {
                if (course.PerformanceAssessment != null) throw new PublicException("Course already has an Performance Assessment.");
                course.PerformanceAssessment = assessment;
            }

            return UpdateCourse(term, course);
        }

        public Course RemoveAssessment(Term term, Course course, AssessmentType type)
        {
            if (type == AssessmentType.Objective)
            {
                course.ObjectiveAssessment = null;
            }
            else
            {
                course.PerformanceAssessment = null;
            }

            return UpdateCourse(term, course);
        }

        private void ValidateCourse(Term term, Course course)
        {
            if (course.Name.IsNull()) throw new PublicException("Must provide a course name.");

            if (course.Start == null || course.End == null) throw new PublicException("Must provide a course start and end date.");

            if (course.Start >= course.End) throw new PublicException("Course start date can not be after course end date.");

            if (course.Instructor == null) throw new PublicException("Must provide course instructor's information.");

            if (course.Instructor.Email.IsValidEmail()) throw new PublicException("Must provide a valid email for course instructor.");

            if (course.Instructor.Phone.IsValidPhoneNumber()) throw new PublicException("Must provide a valid phone number for course instructor.");

            if (term.Start < course.Start) throw new PublicException("Course cannot begin before term start date.");

            if (term.End > course.End) throw new PublicException("Course cannot end after term end date.");
        }

        private void ValidateAssessment(Course course, Assessment assessment)
        {
            if (assessment.Name.IsNull()) throw new PublicException("Assement must have a name.");

            if (assessment.Start >= assessment.End) throw new PublicException("Assements start date must be before the end date.");

            if (course.Start < assessment.Start) throw new PublicException("Assement cannot begin before course start date.");

            if (course.End > assessment.End) throw new PublicException("Assement cannot end after course end date.");
        }
    }
}
