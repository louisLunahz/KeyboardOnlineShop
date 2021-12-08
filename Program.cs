using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LouigisSP.DL;
using LouigisSP.BO;



namespace shoppingPortal
{
    class Program
    {
        static void Main(string[] args)
        {

          


            List<Person> persons = UsersData.GetAllUsers();
            List<Keyboard> products;

            bool validOption;
            int option;

            do {
                Console.Clear();

                Console.WriteLine("++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine("++++++++++++++++WELCOME");
                Console.WriteLine("1.-Sign in ");
                Console.WriteLine("2.-Sign up");
                Console.WriteLine("3.-exit");

                validOption  = int.TryParse(Console.ReadLine(), out option);


                switch (option)
                {
                    case 1:
                        Person person;
                        bool passCorrect;
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("Enter email");
                            person = SearchPersonByEmail(Console.ReadLine(), persons);
                        } while (person == null);
                        do
                        {
                            Console.WriteLine("Enter pass");
                            passCorrect = ComparePass(Console.ReadLine(), person);

                        } while (!passCorrect);


                        //Console.WriteLine("Login succesful");
                        if (person is Admin) {
                            Console.WriteLine("Welcome Admin: "+ person.FirstName+ " "+ person.LastName);
                            Console.ReadKey();
                        } else if (person is Employee) {
                            Console.WriteLine("Welcome Employee: "+person.FirstName+ " "+person.LastName);
                            Console.ReadKey();
                        } else if (person is Customer) {
                            int optionProducts;
                            bool valid;
                            products = ProductsData.getAllProducts();

                            do {

                                Console.Clear();
                                Console.WriteLine("1.-Show all products");
                                Console.WriteLine("2.-Show only Mechanical Keyboards");
                                Console.WriteLine("3.-Show only membrane keyboards");
                                Console.WriteLine("4.-exit");
                                valid = int.TryParse(Console.ReadLine(), out optionProducts);

                                switch (optionProducts)
                                {
                                    case 1:
                                        Console.Clear();
                                        ShowProducts(products);
                                        string idKeyboard;
                                        Keyboard keyboard;
                                       
                                        do
                                        {

                                          
                                            Console.WriteLine(" Enter the id of the product to see full details or r to return ");
                                            idKeyboard = Console.ReadLine();
                                            if (idKeyboard=="r") {
                                                Console.WriteLine("leaving...");
                                                Console.ReadKey();
                                                break;
                                            }

                                            keyboard = GetProduct(products, idKeyboard);
                                            
                                            if (keyboard is null)
                                            {
                                                Console.WriteLine("not found");
                                                Console.ReadKey();
                                            }
                                            else {
                                                keyboard.PrintAllDetails();
                                                Console.ReadKey();
                                            }
                                        } while (idKeyboard=="" || idKeyboard is null) ;




                                            break;

                                    case 2:
                                        Console.Clear();
                                        ShowProducts(products, new MechanicalKeyboard());
                                        Console.ReadKey();
                                        break;

                                    case 3:

                                        Console.Clear();
                                        ShowProducts(products, new Keyboard());
                                        Console.ReadKey();
                                        break;



                                }
                                


                            } while (optionProducts!=4);
                           
                            
                          

                           




                        }



                        break;

                    case 2:
                        person = null;
                        string pass;
                        bool nameCorrect;
                        string email;
                        bool emailCorrect;
                        string shippingAddress;
                        string billingAdreess;
                        bool isValidNumber;
                        string phoneNumber;
                        bool isValidDOB;
                        string firstName;
                        string lastName;
                        string dateOfBirth;


                        Customer customer = new Customer();


                        do
                        {
                            Console.WriteLine("Enter email to register");
                            email = Console.ReadLine();
                            emailCorrect = CheckEmail(email);
                            person = SearchPersonByEmail(email, persons);
                            if (!emailCorrect)
                            {
                                Console.WriteLine("email bad formated");
                            }
                            if (person != null)
                            {
                                Console.WriteLine("email  already registered");
                            }
                        } while (!emailCorrect || !(person is null));
                        customer.Email = email;
                        do
                        {
                            Console.WriteLine("Enter password: ");
                            passCorrect = VerifyPassword(pass = Console.ReadLine());
                        } while (passCorrect == false);

                       
                        do
                        {
                            Console.WriteLine("Enter password again: ");
                            passCorrect = Console.ReadLine() == pass;
                        } while (passCorrect == false);

                        customer.Pass = pass;

                        do
                        {
                            Console.WriteLine("Enter First Name");
                            nameCorrect = CheckName(firstName=Console.ReadLine());
                        } while (nameCorrect == false);
                        customer.FirstName = firstName;
                        do
                        {
                            Console.WriteLine("Enter Last Name");
                            nameCorrect = CheckName(lastName=Console.ReadLine());
                        } while (nameCorrect == false);
                        customer.LastName = lastName;
                        do
                        {
                            Console.WriteLine("Enter shipping Adress");
                            shippingAddress = Console.ReadLine();
                        } while (string.IsNullOrEmpty(shippingAddress));
                        customer.ShippingAddress = shippingAddress;
                        
                        do
                        {
                            Console.WriteLine("Enter billing Adress");
                            billingAdreess = Console.ReadLine();
                        } while (string.IsNullOrEmpty(billingAdreess));
                        customer.BillingAddress = billingAdreess;
                        do
                        {
                            Console.WriteLine("Enter your phone number");
                            isValidNumber = CheckPhoneNumber(phoneNumber = Console.ReadLine());
                        } while (!isValidNumber);

                        customer.PhoneNumber = phoneNumber;
                        do {
                            Console.WriteLine("Enter your date of Birth");
                            isValidDOB = CheckDateOfBirth(dateOfBirth=Console.ReadLine());
                        } while (!isValidDOB);
                        customer.DateOfBirth = DateTime.Parse(dateOfBirth);
                        UsersData.AddPerson(customer);
                        Console.WriteLine("Customer registered succesfully");
                        Console.ReadKey();
         

                        break;
                }



            } while (option!=3 );

           





            


        }



        //public static Person SearchPersonByEmail(string email, List<Person>persons) {
        //    Person person=null;
        //    foreach (Person p in persons) {
        //        if (email==p.Email) {
        //            person = p;
        //            break;
        //        }
        //    }
        //    return person;
        //}

        public static Person SearchPersonByEmail(string email, List<Person>persons) {
           Person per  =
               (from person in persons
                where person.Email == email
                select person).SingleOrDefault<Person>();
            return per;

            
        }

        public static bool ComparePass(string pass, Person person) {
            if (pass == person.Pass)
            {
                return true;
            }
            else return false;
        }

        public static bool VerifyPassword(string password) {
            
            string hasNumber = @"[0-9]+";
            string hasUpperChar = @"[A-Z]+";
            string hasMinimum8Chars = @".{8,}";


            return (Regex.IsMatch(password, hasNumber) && Regex.IsMatch(password, hasUpperChar) && Regex.IsMatch(password, hasMinimum8Chars));
            
        }

        public static bool CheckName(string name) {
            bool isItCorrect;
            if (name.Length <= 3)
            {
                isItCorrect = false;
            }
            else isItCorrect = true;

            
            
            
            

            return isItCorrect;
        }

        public static bool CheckEmail(string email) {
            bool isEmailCorrect;
           isEmailCorrect= Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return isEmailCorrect;
        }

        public static bool CheckPhoneNumber(string phone) {
            string pattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
            if (phone != null)
            {
                return Regex.IsMatch(phone, pattern);
            }
            else return false;
        }


        //check for the date to be not null, well formated and for the user not being older than 100 years
        public static bool CheckDateOfBirth(string DOB) {
            bool validDateOfBirth=false;
            TimeSpan maxAgeAccepted = new TimeSpan(36500,0,0,0); //Max age accepted is 36500 days
            if (DOB != null) {
                DateTime date_dateOfBirth;
               bool dateWellParsed= DateTime.TryParse(DOB, out date_dateOfBirth);

                TimeSpan timeDifference = DateTime.Now.Subtract(date_dateOfBirth);
                if ( dateWellParsed && timeDifference<=maxAgeAccepted) {
                    validDateOfBirth = true;
                }
            


            }
            return validDateOfBirth;
        }


        public static void ShowProducts(List<Keyboard> producs) {
            foreach (Keyboard k in producs) {
                Console.WriteLine(k.ToString());
            }
        }

        public static void ShowProducts(List<Keyboard> producs, Keyboard keyboard) {
            foreach (Keyboard k in producs)
            {
                if (k.GetType()==keyboard.GetType()) {
                    Console.WriteLine(k.ToString());
                }
                
            }
        }

        public static Keyboard GetProduct(List<Keyboard> products ,string id){
            Keyboard kbd =
                (from keyboard in products
                where keyboard.Id == id
                select keyboard).SingleOrDefault<Keyboard>();
            return kbd;
        }












    }
}
