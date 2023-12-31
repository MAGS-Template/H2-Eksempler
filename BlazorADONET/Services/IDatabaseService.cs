namespace BlazorADONET.Services
{
    using Domain_Models;
    using System;

    public interface IDatabaseService
    {
        List<Class> GetAllClassesWithStudentsAndTeachers();
        void CreateClass(Class cls);
        Class GetClassById(int id);
        void UpdateClass(Class cls);
        void DeleteClass(int id);
    }
}