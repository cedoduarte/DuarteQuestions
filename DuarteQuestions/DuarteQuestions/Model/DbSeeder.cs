using DuarteQuestions.Utils;

namespace DuarteQuestions.Model
{
    public static class DbSeeder
    {
        public static void DoSeeding(AppDbContext? dbContext)
        {
            SeedUsers(dbContext);
            SeedQuestions(dbContext);
        }

        private static void SeedUsers(AppDbContext? dbContext)
        {
            if (!dbContext!.Users.Any())
            {
                var users = new User[]
                {
                    new User()
                    {
                        Name = "carlos",
                        Email = "carlosduarte.1@hotmail.com",
                        Password = Util.ToSHA256("root")
                    },
                    new User()
                    {
                        Name = "jesus",
                        Email = "jesus@hotmail.com",
                        Password = Util.ToSHA256("root")
                    }
                };
                foreach (User user in users)
                {
                    dbContext.Users.Add(user);
                }
                dbContext.SaveChanges();
            }
        }

        private static void SeedQuestions(AppDbContext? dbContext)
        {
            if (!dbContext!.Questions.Any())
            {
                var answers = new Answer[]
                {
                    new Answer { Id = 1, Text = "Es el hijo de Dios" },
                    new Answer { Id = 2, Text = "Es una persona desconocida" },
                    new Answer { Id = 3, Text = "Es un simple carpintero sin futuro" },

                    new Answer { Id = 4, Text = "Su misión es vivir de forma egoista" },
                    new Answer { Id = 5, Text = "Su misión es abandonar todo lo que conoce" },
                    new Answer { Id = 6, Text = "Su misión es ser libre para cumplir su propósito" },

                    new Answer { Id = 7, Text = "Porque es una virtud necesaria" },
                    new Answer { Id = 8, Text = "No es necesario tener paciencia" },
                    new Answer { Id = 9, Text = "No creo que la paciencia sea buena" },

                    new Answer { Id = 10, Text = "No es necesario viajar" },
                    new Answer { Id = 11, Text = "Hay que viajar a Estados Unidos y nunca volver" },
                    new Answer { Id = 12, Text = "Hay que viajar al sur de México y nunca volver" },

                    new Answer { Id = 13, Text = "No irá nadie al viaje" },
                    new Answer { Id = 14, Text = "Solo una persona puede ir al viaje" },
                    new Answer { Id = 15, Text = "Toda la familia puede ir al viaje" }
                };

                var questions = new Question[]
                {
                    new Question
                    {
                        Id = 1,
                        Text = "¿Quién es Jesucristo?",
                        Answers = new List<Answer>
                        {
                            answers[0],
                            answers[1],
                            answers[2]
                        },
                        RightAnswer = answers[0]
                    },
                    new Question
                    {
                        Id = 2,
                        Text = "¿Cuál es la misión de Juan?",
                        Answers = new List<Answer>
                        {
                            answers[3],
                            answers[4],
                            answers[5]
                        },
                        RightAnswer = answers[5]
                    },
                    new Question
                    {
                        Id = 3,
                        Text = "¿Por qué hay que tener paciencia?",
                        Answers = new List<Answer>
                        {
                            answers[6],
                            answers[7],
                            answers[8]
                        },
                        RightAnswer = answers[6]
                    },
                    new Question
                    {
                        Id = 4,
                        Text = "¿A dónde hay que viajar?",
                        Answers = new List<Answer>
                        {
                            answers[9],
                            answers[10],
                            answers[11]
                        },
                        RightAnswer = answers[11]
                    },
                    new Question
                    {
                        Id = 5,
                        Text = "¿Cuántas personas irán en el viaje?",
                        Answers = new List<Answer>
                        {
                            answers[12],
                            answers[13],
                            answers[14]
                        },
                        RightAnswer = answers[13]
                    }
                };

                foreach (Answer answer in answers)
                {
                    dbContext.Answers.Add(answer);
                }
                dbContext.SaveChanges();

                foreach (Question question in questions)
                {
                    dbContext.Questions.Add(question);
                }
                dbContext.SaveChanges();
            }
        }
    }
}
