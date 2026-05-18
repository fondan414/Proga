public class Student
{
    private string name;
    private int id;
    private string group;

    public Student(string name, int id, string group)
    {
        this.name = name;
        this.id = id;
        this.group = group;
    }

    public string Name { get => name; set => name = value; }
    public int Id { get => id; set => id = value; }
    public string Group { get => group; set => group = value; }

    public override bool Equals(object obj) => obj is Student other && id == other.id;
    public override int GetHashCode() => id.GetHashCode();
    public override string ToString() => $"ID: {id}, Имя: {name}, Группа: {group}";
}

public class BonusActivity
{
    private string activityName;
    private int points;

    public BonusActivity(string activityName, int points)
    {
        this.activityName = activityName;
        this.points = points;
    }

    public string ActivityName { get => activityName; set => activityName = value; }
    public int Points { get => points; set => points = value; }
    public override string ToString() => $"{activityName}:{points}";
}

public class RatingManager
{
    private Dictionary<Student, List<BonusActivity>> studentActivities;

    public RatingManager() => studentActivities = new Dictionary<Student, List<BonusActivity>>();

    public void AddStudent(Student student)
    {
        if (!studentActivities.ContainsKey(student))
            studentActivities[student] = new List<BonusActivity>();
    }

    public void AddActivityToStudent(Student student, BonusActivity activity)
    {
        if (!studentActivities.ContainsKey(student))
            throw new ArgumentException($"Студент с ID {student.Id} не найден в системе");
        studentActivities[student].Add(activity);
    }

    public int GetTotalStudentPoints(Student student)
    {
        if (!studentActivities.ContainsKey(student))
            throw new ArgumentException($"Студент с ID {student.Id} не найден в системе");
        
        var x = 0;
        var y = 0;
        foreach (var qq in studentActivities[student])
        {
            x = x + 1;
            y = y + qq.Points;
        }
        return y;
    }

    public IEnumerable<Student> GetAllStudentsAbovePoints(int threshold)
    {
        var list1 = new List<Student>();
        foreach (var z in studentActivities.Keys)
        {
            var tt = GetTotalStudentPoints(z);
            if (tt > threshold)
            {
                list1.Add(z);
            }
        }
        return list1;
    }

    public void SaveToFile(string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var pair in studentActivities)
            {
                var bb = pair.Key.Id.ToString();
                var cc = pair.Key.Name;
                var dd = pair.Key.Group;
                var ee = string.Join(";", pair.Value.Select(a => a.ToString()));
                var result = $"{bb}|{cc}|{dd}|{ee}";
                writer.WriteLine(result);
            }
        }
    }

    public void LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Файл {filePath} не найден");

        studentActivities.Clear();
        
        using (StreamReader reader = new StreamReader(filePath))
        {
            var line = "";
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) 
                    continue;
                
                var parts = line.Split('|');
                if (parts.Length != 4) 
                    continue;
                
                var s1 = new Student(parts[1], int.Parse(parts[0]), parts[2]);
                var acts = new List<BonusActivity>();
                
                if (!string.IsNullOrEmpty(parts[3]))
                {
                    var actStrs = parts[3].Split(';');
                    foreach (var entry in actStrs)
                    {
                        var actParts = entry.Split(':');
                        if (actParts.Length == 2)
                            acts.Add(new BonusActivity(actParts[0], int.Parse(actParts[1])));
                    }
                }
                studentActivities[s1] = acts;
            }
        }
    }

    public void DisplayAllStudentsInfo()
    {
        foreach (var p in studentActivities)
        {
            Console.WriteLine(p.Key);
            var pp = GetTotalStudentPoints(p.Key);
            Console.WriteLine($"Общее количество баллов: {pp}");
            Console.WriteLine("Активности:");
            foreach (var act in p.Value)
                Console.WriteLine($"  - {act}");
            Console.WriteLine();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        try
        {
            var mm = new RatingManager();
            
            var st1 = new Student("Иванов Иван", 1001, "Группа А");
            var st2 = new Student("Педрова Мария", 1002, "Группа А");
            var st3 = new Student("Сидоров Алексей", 1003, "Группа Б");
            var st4 = new Student("Козлова Елена", 1004, "Группа Б");
            
            mm.AddStudent(st1);
            mm.AddStudent(st2);
            mm.AddStudent(st3);
            mm.AddStudent(st4);
            
            var aa1 = new BonusActivity("Олимпиада по программированию", 50);
            var aa2 = new BonusActivity("Проект по базам данных", 30);
            var aa3 = new BonusActivity("Активность на паре", 10);
            
            mm.AddActivityToStudent(st1, aa1);
            mm.AddActivityToStudent(st1, aa2);
            mm.AddActivityToStudent(st1, aa3);
            
            var bb1 = new BonusActivity("Олимпиада по математике", 40);
            var bb2 = new BonusActivity("Волонтерская деятельность", 25);
            
            mm.AddActivityToStudent(st2, bb1);
            mm.AddActivityToStudent(st2, bb2);
            
            var cc1 = new BonusActivity("Проект по веб-разработке", 35);
            var cc2 = new BonusActivity("Участие в конференции", 45);
            var cc3 = new BonusActivity("Активность на паре", 15);
            var cc4 = new BonusActivity("Дополнительный проект", 20);
            
            mm.AddActivityToStudent(st3, cc1);
            mm.AddActivityToStudent(st3, cc2);
            mm.AddActivityToStudent(st3, cc3);
            mm.AddActivityToStudent(st3, cc4);
            
            var dd1 = new BonusActivity("Олимпиада по информатике", 55);
            mm.AddActivityToStudent(st4, dd1);
            
            Console.WriteLine("\nИнформация обо всех студентах:");
            mm.DisplayAllStudentsInfo();
            
            var t1 = mm.GetTotalStudentPoints(st1);
            var t3 = mm.GetTotalStudentPoints(st3);
            
            Console.WriteLine($"Сумма баллов студента {st1.Name}: {t1}");
            Console.WriteLine($"Сумма баллов студента {st3.Name}: {t3}");
            
            var th = 80;
            Console.WriteLine($"\nСтуденты с баллами выше {th} :");
            var topStudents = mm.GetAllStudentsAbovePoints(th);
            foreach (var student in topStudents)
                Console.WriteLine($"{student.Name} - {mm.GetTotalStudentPoints(student)} баллов");
            
            var fp = "students_data.txt";
            mm.SaveToFile(fp);
            Console.WriteLine($"\nДанные сохранены в файл: {fp}");
            
            var nm = new RatingManager();
            nm.LoadFromFile(fp);
            Console.WriteLine("\nДанные загружены из файла");
            Console.WriteLine("\nИнформация обо всех студентах ИЗ ФАЙЛА:");
            nm.DisplayAllStudentsInfo();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Общая ошибка: {ex.Message}");
        }
    }
}
