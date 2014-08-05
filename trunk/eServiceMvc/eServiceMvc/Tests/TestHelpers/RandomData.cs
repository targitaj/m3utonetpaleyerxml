namespace Uma.Eservices.TestHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Globalization;
    using System.Text;
    using System.Web.Mvc;

    /// <summary>
    /// This class is capable of creating Random Data in form of strings (as Words, Sentences, Paragraphs based on several parameters - ad-hoc settings).
    /// Also contains methods to return random Enum value, random bool value, random Objects and specific (non Business) objects.
    /// Class also provides static Random instance for use outside of this class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class RandomData
    {
        #region [Private Fields]

        /// <summary>
        /// Randomizer for class usage - getting random data
        /// </summary>
        private static Random randomSeeder;

        /// <summary>
        /// Uppercase letters to be used in generated data
        /// </summary>
        private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Lowercase letters to be used in generated data
        /// </summary>
        private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// Numbers to be used in generated data
        /// </summary>
        private const string Numbers = "0123456789";

        /// <summary>
        /// Other Symbols, that can be used by data generator
        /// </summary>
        private const string Symbols = @"~`@#$%^&*()-_=+<>:/\[]{}|'";

        /// <summary>
        /// Punctuation marks, that can be used in generator
        /// </summary>
        private const string Punctuation = @".,!?";

        /// <summary>
        /// Non-English uppercase characters
        /// </summary>
        private const string UppercaseLocalized = "ĀČĒĢĪĶĻŅŠŪŽÀÁÁÄÈÉÌÍÒÓÕÖÜßЙЦУКЕНВАМИПРО";

        /// <summary>
        /// Non-english lowercase characters
        /// </summary>
        private const string LowercaseLocalized = "āčēģīķļņšūžàáäèéìíõöùúüęįйцукенвамипро";

        /// <summary>
        /// Defines default character sets to generate outcome
        /// </summary>
        private const StringIncludes DefaultIncludes =
            StringIncludes.Uppercase | StringIncludes.Lowercase | StringIncludes.Numbers;

        #region [Person-Male First Names]
        private static string[] maleFirstNames = {
            "Luciano", "Ričards", "Darío", "Zbigniew", "Алексей", "Gérard", "Вячеслав", "Константин", "Владимир",
            "Zacarías", "Bård", "Žoržs", "Jacques", "Étienne", "Vytautas", "Тимофей", "Valērijs", "Павел", "Ryöpetti",
            "Krišjānis", "Antonio", "Uwe", "Григорий", "Алëша", "René", "Юрий", "Xavier", "Åsmund", "Łukasz", "Léon",
            "Esbjørn", "César", "Геннадий", "Сергей", "Fabio", "Väiko", "André", "Väinämö", "Игорь", "Евгений", "Jokūbas",
            "Иван", "Guillermo", "Jürgen", "Andrzej", "Alessandro", "Леонид", "Jerónimo", "Jarosław", "Matthäus", "Erčius",
            "Wolfgang", "Giordano", "Sævar", "Hågen", "Miķelis", "Heinrich", "Олег", "José", "Ģirts", "Виктор", "Donato",
            "Werner", "Götz", "Claudio", "Giuseppe", "Даниил", "Børge", "Валерий", "Фёдор", "Günter", "Николай", "Bruno",
            "Enrique", "Василий", "François", "Marcello", "Tõivo", "Станислав", "Broņislavs", "Sebastián", "Борис", "Fabián",
            "Zigmārs", "Андрей", "Sõlmi", "Józef", "Adriano", "Czesław", "Fabrizio", "Raúl", "Leonardo", "Oļegs", "Герман",
            "Šarunas", "Максим", "Björn", "Михаил", "Ægir", "Mārtiņš", "Østen", "Česlavs"
            };
        #endregion
        #region [Person-Female First names]
        private static string[] femaleFirstNames = {
            "Людмила", "Наташа", "Salomé", "Margaux", "Małgorzata", "Полина", "Жанна", "Paola", "Анфиса", "Валерия",
            "Sölvi", "Алиса",  "Вероника", "Luciana", "Džūlija", "Ольга", "Ирина", "Aída", "Thérèse", "Cécile", "Mercedes",
            "Ксения", "Grażyna", "Nicoletta", "Таня", "Катя",  "Geneviève", "Ģertrūde", "Chiquita", "Галина", "Michèle",
            "Наталья", "Õnnela", "Лидия", "Mónica", "Giedrė", "Penélope", "Инна", "Надежда", "Ülve", "Angélica",
            "Bridžita", "Ramūnė", "Häidi", "Rosanna", "Татьяна", "Ornella", "Žermēna", "Евгения", "Маша", "Aļesja",
            "Eglė", "Софья", "Øllegård", "Žyvilė", "Jürja", "Amélie", "Õile",  "Екатерина", "Žaklīna", "Lærke", "Светлана",
            "Ērika", "Aurélie", "Виктория", "Lønne", "Александра", "Débora", "Adélaïde",  "Sølva", "Īrisa", "Sæunn",
            "Елизавета", "Зинаида", "Verónica", "Yazmín", "Антония", "Юлия", "Bärbel", "Тамара", "Pénélope",
        };
        #endregion
        #region [Person Surnames]
        private static string[] personSurnames = {
            "Смирнов", "Иванов", "Кузнецов", "Соколов", "Попов", "Лебедев", "Козлов", "Новиков", "Морозов", 
            "Петров", "Волков", "Соловьёв", "Васильев", "Зайцев", "Павлов", "Семёнов", "Голубев", "Виноградов", 
            "Богданов", "Воробьёв", "Фёдоров", "Михайлов", "Беляев", "Тарасов", "Белов", "Комаров", 
            "Müller", "Hodžić", "Čengić", "Delić", "Kovačević", "Tahirović", "Šarić", "Barišić", "Mészáros",
            "Novák", "Černý", "Němec", "Růžička", "Sedláček", "Horák", "Dvořák", "Pospíšil", "Hájek",
            "Sørensen", "Jørgensen", "Mägi", "Pärn", "Saar", "Højgaard", "Mäkinen", "Hämäläinen", "Järvinen",
            "Mäkelä", "Heikkilä", "Moreau", "Lefèvre", "François", "Papadopoulos", "Szabó", "Takács",
            "O'Sullivan", "McCarthy", "O'Neill", "O'Connor", "De Luca", "Rossi", "Thaçi", 
            "Bērziņš", "Krūmiņš", "Eglītis", "Pētersons", "Vītoliņš", "Kļaviņš", "Kārkliņš", 
            "Ķēniņš", "Ņečajevs", "Ļūdēns", "Šmits", "Čikste", "Žukauskas", "Stankevičius",
            "Paulauskienė", "Kazlauskienė", "Van Dijk", "Van der Meer", "Kristiansen", "Kamiński",
            "Wiśniewski", "Dąbrowski", "Kozłowski", "Wojciechowski", "López", "Hernández", "Álvarez",
            "Domínguez", "Suárez", "Johansson", "Karlsson", "Harutyunyan", "Məmmədov", "Hüseynov",
            "Süleymanov", "Chén", "Lǐ", "Zhāng", "Liú", "Wáng", "Yáng", "Zhōu", "Guō", "Beridze",
            "Gelashvili", "Giorgadze", "Singh", "Kumar", "Gandhi", "Avraham", "Shapira", "Çelik",
            "Satō", "Takashi", "Yamamoto", "Yamaguchi", "Öztürk", "Şahin", "Yıldırım", "Erdoğan"
        };
        #endregion
        #endregion

        #region [Include Controlling Enumerator]
        /// <summary>
        /// Flagged enumerator for specifying which character sets should be used in string creation.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "This is only used by this generator"), Flags]
        public enum StringIncludes
        {
            /// <summary>
            /// Empty. Do not use this option, as this produces empty strings
            /// </summary>
            None = 0x0,

            /// <summary>
            /// Include English uppercase letters in generated strings
            /// </summary>
            Uppercase = 0x1,

            /// <summary>
            /// Include English lowercase letters in generated strings
            /// </summary>
            Lowercase = 0x2,

            /// <summary>
            /// Include symbols (!@#^%$) in generated strings
            /// </summary>
            Symbols = 0x4,

            /// <summary>
            /// Include pubctuation parks (dot, comma, exclamation sign) in generated strings
            /// </summary>
            Punctuation = 0x8,

            /// <summary>
            /// Include numbers (0..9) in generated strings
            /// </summary>
            Numbers = 0x10,

            /// <summary>
            /// Include Non-English uppercase letters in generated strings
            /// </summary>
            LocalizedUppercase = 0x20,

            /// <summary>
            /// Include Non-English lowercase letters in generated strings
            /// </summary>
            LocalizedLowercase = 0x40
        }

        #endregion

        /// <summary>
        /// Provides static instance of <see cref="Random"/> for external use.
        /// </summary>
        public static Random RandomSeed
        {
            get { return randomSeeder ?? (randomSeeder = new Random()); }
        }

        #region [Random String values]
        /// <summary>
        /// Creates the random string with length from 5 to 25 characters.
        /// String consists of uppercase, lowercase letters and numbers.
        /// </summary>
        /// <returns>Random string in lenght between 5 and 25 characters (uppercase/lowercase letters and numbers)</returns>
        public static string GetString()
        {
            return GetString(5, 25);
        }

        /// <summary>
        /// Creates the random number as string. To avoid cast problems, first character is never Zero "0".
        /// User should specify length of number, like for "4", it should return "8346"
        /// </summary>
        /// <param name="length">The length of characterized number.</param>
        /// <returns>Number as character string, like "70457394".</returns>
        public static string GetStringNumber(int length)
        {
            if (length == 0)
            {
                throw new ArgumentException("Length should not be less than 1 for RandomNumberAsString method", "length");
            }

            // Make first character always within 1-9
            string firstCharacter = RandomSeed.Next(1, 9).ToString(CultureInfo.InvariantCulture);
            if (length > 1)
            {
                return firstCharacter + GetString(length - 1, length - 1, StringIncludes.Numbers);
            }

            return firstCharacter;
        }

        /// <summary>
        /// Creates the random string with length from 4 to 14 characters.
        /// String is made of lowercase letters, including non-english letters.
        /// Example: ķēniņš, läätöö
        /// </summary>
        /// <returns>Random string in length between 4 and 14 characters (lowercase+localized lowercase letters only)</returns>
        public static string GetStringWord()
        {
            return GetString(4, 14, StringIncludes.Lowercase | StringIncludes.LocalizedLowercase);
        }

        /// <summary>
        /// Creates the random string with length from 4 to 14 characters.
        /// String consists of first uppercase and all the rest - lowercase (also non-english) letters only.
        /// Example: Björklund, Supermän
        /// </summary>
        /// <returns>A word in Proper form to use as human or city names, like "Huālfdl"</returns>
        public static string GetStringWordProper()
        {
            return GetString(1, 1, StringIncludes.Uppercase | StringIncludes.LocalizedUppercase) + GetString(3, 13, StringIncludes.Lowercase | StringIncludes.LocalizedLowercase);
        }

        /// <summary>
        /// Creates the random wording sentence according to specified parameters.
        /// </summary>
        /// <param name="wordCount">The count of words within sentence.</param>
        /// <param name="addPunctuation">If set to <c>true</c> will finish sentence with some punctuation mark (dot, exclamation).</param>
        /// <param name="useLocalLetters">If set to <c>true</c> will use also localized (non-english)  letters.</param>
        /// <returns>Sentence of random words according to parameters</returns>
        public static string GetStringSentence(int wordCount, bool addPunctuation, bool useLocalLetters)
        {
            StringIncludes includes = StringIncludes.Lowercase;
            if (useLocalLetters)
            {
                includes |= StringIncludes.LocalizedLowercase;
            }

            StringBuilder sentence = new StringBuilder();
            int wordCounter = 0;
            sentence.Append(GetString(1, 1, StringIncludes.Uppercase));
            while (wordCounter < wordCount)
            {
                if (wordCounter > 0)
                {
                    sentence.Append(" ");
                }

                sentence.Append(GetString(4, 14, includes));
                wordCounter++;
            }

            if (addPunctuation)
            {
                sentence.Append(GetString(1, 1, StringIncludes.Punctuation));
            }

            return sentence.ToString();
        }

        /// <summary>
        /// Creates the random paragraph of specified count of sentences (<see cref="GetStringSentence"/>).
        /// </summary>
        /// <param name="countOfSentences">The count of sentences in paragraph.</param>
        /// <param name="useLocalLetters">If set to <c>true</c> then wording will include localized letters.</param>
        /// <returns>Creates a paragraf - several sentences consisting of random count of random words.</returns>
        public static string GetStringParagraph(int countOfSentences, bool useLocalLetters)
        {
            StringBuilder paragraph = new StringBuilder();
            int sentenceCounter = 0;
            while (sentenceCounter < countOfSentences)
            {
                if (sentenceCounter > 0)
                {
                    paragraph.Append(" ");
                }

                paragraph.Append(GetStringSentence(RandomSeed.Next(5, 10), true, useLocalLetters));
                sentenceCounter++;
            }

            paragraph.Append(Environment.NewLine);
            return paragraph.ToString();
        }

        /// <summary>
        /// Creates the random string of specified length.
        /// String consists of uppercase, lowercase letters and numbers.
        /// </summary>
        /// <param name="length">The length of required string</param>
        /// <returns>Random string of specified lenght (uppercase/lowercase letters and numbers)</returns>
        public static string GetString(int length)
        {
            return GetString(length, length, DefaultIncludes);
        }

        /// <summary>
        /// Creates the random string of specified length.
        /// String consists of uppercase, lowercase letters and numbers plus localized letters if specified by attribute.
        /// </summary>
        /// <param name="length">The length of generated string</param>
        /// <param name="includeLocalized">If set to <c>true</c> generated string will also include localized letters.</param>
        /// <returns>Random string of specified lenght (uppercase/lowercase letters and numbers and localized letters if set by parameter)</returns>
        public static string GetString(int length, bool includeLocalized)
        {
            var includes = DefaultIncludes;
            if (includeLocalized)
            {
                includes |= StringIncludes.LocalizedUppercase;
                includes |= StringIncludes.LocalizedLowercase;
            }

            return GetString(length, length, includes);
        }

        /// <summary>
        /// Creates the random string of specified length span.
        /// String consists of uppercase, lowercase letters and numbers.
        /// </summary>
        /// <param name="minimumLength">The minimum length of generated string</param>
        /// <param name="maximumLength">The maximum length of generated string</param>
        /// <returns>Random string of specified length (uppercase/lowercase letters and numbers)</returns>
        public static string GetString(int minimumLength, int maximumLength)
        {
            return GetString(minimumLength, maximumLength, DefaultIncludes);
        }

        /// <summary>
        /// Creates the random string within specified length parameters and by using specified character sets
        /// </summary>
        /// <param name="minimumLength">The minimum length of returned string</param>
        /// <param name="maximumLength">The maximum length of returned string</param>
        /// <param name="generatorParameters">The character sets that needs to be included/used in generated string</param>
        /// <returns>Random string of specified length and character sets.</returns>
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:CurlyBracketsMustNotBeOmitted", Justification = "No need to extend code.")]
        public static string GetString(int minimumLength, int maximumLength, StringIncludes generatorParameters)
        {
            if (minimumLength == 0)
                throw new ArgumentException("GenericStringGenerator.GetString was called with miminumLength < 1", "minimumLength");
            if (maximumLength == 0)
                throw new ArgumentException("GenericStringGenerator.GetString was called with maximumLength < 1", "maximumLength");
            if (maximumLength < minimumLength)
                throw new ArgumentException("GenericStringGenerator.GetString was called with miminumLength > maximumLength", "minimumLength");
            if (generatorParameters == 0)
                throw new ArgumentException("GenericStringGenerator.GetString was called without specifying parameters", "generatorParameters");

            int randomLength = RandomSeed.Next(minimumLength, maximumLength);
            char[] charArray = new char[randomLength];
            string charPool = String.Empty;

            // Build character pool
            if (generatorParameters.HasFlag(StringIncludes.Lowercase))
            {
                charPool += Lowercase;
            }

            if (generatorParameters.HasFlag(StringIncludes.Uppercase))
            {
                charPool += Uppercase;
            }

            if (generatorParameters.HasFlag(StringIncludes.Numbers))
            {
                charPool += Numbers;
            }

            if (generatorParameters.HasFlag(StringIncludes.Symbols))
            {
                charPool += Symbols;
            }

            if (generatorParameters.HasFlag(StringIncludes.Punctuation))
            {
                charPool += Punctuation;
            }

            if (generatorParameters.HasFlag(StringIncludes.LocalizedLowercase))
            {
                charPool += LowercaseLocalized;
            }

            if (generatorParameters.HasFlag(StringIncludes.LocalizedUppercase))
            {
                charPool += UppercaseLocalized;
            }

            // Build the output character array
            for (int i = 0; i < charArray.Length; i++)
            {
                // Pick a random integer in the character pool
                int index = RandomSeed.Next(0, charPool.Length);

                // Set it to the output character array
                charArray[i] = charPool[index];
            }

            return new string(charArray);
        }
        #endregion

        #region [Random Person Names]

        /// <summary>
        /// Gets the first name of a Person from predefined list of actual names
        /// </summary>
        /// <param name="isMale">If TRUE - returns Male (maskuline) name, otherwise - female (feminine) name.</param>
        /// <returns>Name of a person in form of one or two actual person names with special characters in them.</returns>
        public static string GetStringPersonFirstName(bool isMale = true)
        {
            if (isMale)
            {
                return GetStringPersonFirstName(maleFirstNames);
            }

            return GetStringPersonFirstName(femaleFirstNames);
        }

        /// <summary>
        /// Gets the person surname out of predefined list of actual person surnames from all over the world.
        /// </summary>
        /// <param name="isMale">If TRUE - returns Male (maskuline) name, otherwise - female (feminine) name.</param>
        /// <returns>Either single person surname or double surname, separated by "-"</returns>
        public static string GetStringPersonLastName(bool isMale = true)
        {
            string personSurname = personSurnames[RandomSeed.Next(personSurnames.Length - 1)];

            // 30% cases return Double Name (Wolfgang Amadeus)
            if (RandomSeed.Next(10) > (isMale ? 8 : 5))
            {
                personSurname = String.Format("{0} - {1}", personSurname, personSurnames[RandomSeed.Next(personSurnames.Length - 1)]);
            }

            return personSurname;
        }

        /// <summary>
        /// Gets the random first name of person out of supplied array of predefined names
        /// </summary>
        /// <param name="sourceNames">The source names array.</param>
        /// <returns>Picked one or two names out of supplied array</returns>
        private static string GetStringPersonFirstName(string[] sourceNames)
        {
            string personName = sourceNames[RandomSeed.Next(sourceNames.Length - 1)];

            // 30% cases return Double Name (Wolfgang Amadeus)
            if (RandomSeed.Next(10) > 6)
            {
                personName = String.Format("{0} {1}", personName, sourceNames[RandomSeed.Next(sourceNames.Length - 1)]);
            }

            return personName;
        }

        /// <summary>
        /// Returns the random Address string, more or less reminding of a real address.
        /// Uses American address style, like "323 Willow street, SE", but number, street name are random values
        /// Note: street name contains non-english characters
        /// </summary>
        public static string GetStringAddress()
        {
            return string.Format("{0} {1} street, {2}", GetStringNumber(3), GetStringWordProper(), GetParameter<string>("SE", "SW", "NE", "NW"));
        }
        #endregion

        #region [Random DateTime values]
        /// <summary>
        /// Returns random DateTime within humanly possible time period -90/+7 years from today.
        /// </summary>
        /// <returns>Humanly bearable date</returns>
        public static DateTime GetDateTime()
        {
            return GetDateTime(DateTime.Now.AddYears(-90), DateTime.Now.AddYears(7));
        }

        /// <summary>
        /// Gets the random date time value in past. Date can be used for Birthday, as it does not goes more than 90 years in past
        /// </summary>
        /// <returns>Date in past —90 years...today</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "It is better to be a method for usage sake.")]
        public static DateTime GetDateTimeInPast()
        {
            return GetDateTime(DateTime.Now.AddYears(-90), DateTime.Today.AddDays(-1));
        }

        /// <summary>
        /// Returns random date of an Adult: 18-80 years old for today's date
        /// </summary>
        /// <returns>Date, that can be random adult birthday</returns>
        public static DateTime GetDateTimeAdultBirthday()
        {
            return GetDateTime(DateTime.Now.AddYears(-80), DateTime.Today.AddYears(-18));
        }

        /// <summary>
        /// Gets the random date for up to 7 years in future (and no earlier/starting next week)
        /// </summary>
        /// <returns>DateTime value (random), that is somewhere in future (>Today ... 7 years)</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "It is better to be a method for usage sake.")]
        public static DateTime GetDateTimeInFuture()
        {
            return GetDateTime(DateTime.Now.AddMinutes(5), DateTime.Today.AddYears(7));
        }

        /// <summary>
        /// Returns the Random DateTime value between two specified.
        /// </summary>
        /// <param name="minimumDate">The minimum date allowed</param>
        /// <param name="maximumDate">The maximum date allowed</param>
        /// <returns>Date within specified values</returns>
        public static DateTime GetDateTime(DateTime minimumDate, DateTime maximumDate)
        {
            if (maximumDate <= minimumDate)
            {
                throw new ArgumentException("MaximumaDate must be greater than MinimumDate");
            }

            TimeSpan range = new TimeSpan(maximumDate.Ticks - minimumDate.Ticks);
            DateTime calculatedDate = minimumDate + new TimeSpan((long)(range.Ticks * RandomSeed.NextDouble()));
            return calculatedDate.Date.AddHours(calculatedDate.Hour).AddMinutes(calculatedDate.Minute);
        }
        #endregion

        #region [Various Randoms]
        /// <summary>
        /// Returns the random integer value between specified range. It is just wrapper for Random.Next
        /// </summary>
        /// <param name="minimumValue">The minimum value allowed</param>
        /// <param name="maximumValue">The maximum value allowed</param>
        /// <returns>Random Integer number between specified values</returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "integer", Justification = "It is especially needed to have name Bool in this method.")]
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "It is better to be a method for usage sake.")]
        public static int GetInteger(int minimumValue, int maximumValue)
        {
            return RandomSeed.Next(minimumValue, maximumValue);
        }

        /// <summary>
        /// Funny method to get "any integer"
        /// </summary>
        private static int GetInt32()
        {
            unchecked
            {
                int firstBits = RandomSeed.Next(0, 1 << 4) << 28;
                int lastBits = RandomSeed.Next(0, 1 << 28);
                return firstBits | lastBits;
            }
        }

        /// <summary>
        /// Returns completely random Decimal number.
        /// Note: Distribution of decimals here is not uniform.
        /// </summary>
        public static decimal GetDecimal()
        {
            byte scale = (byte)RandomSeed.Next(29);
            bool sign = RandomSeed.Next(2) == 1;
            return new decimal(GetInt32(),
                               GetInt32(),
                               GetInt32(),
                               sign,
                               scale);
        }

        /// <summary>
        /// Returns random boolean value - either true or false.
        /// </summary>
        /// <returns>Either true or false, you'll never know</returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bool", Justification = "It is especially needed to have name Bool in this method.")]
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "It is better to be a method for usage sake.")]
        public static bool GetBool()
        {
            return RandomSeed.NextDouble() > 0.5;
        }

        /// <summary>
        /// Gets the random enumeration value from specified Enum values.
        /// </summary>
        /// <typeparam name="T">Specified Enumeration class</typeparam>
        /// <param name="exceptZero">If set to <c>true</c> - returns enum values starting from second (not zero value, which might be NA/Not Use). Default = false</param>
        /// <returns>
        /// Random enumeration value from specified enum class
        /// </returns>
        public static T GetEnum<T>(bool exceptZero = false)
        {
            T[] values = (T[])Enum.GetValues(typeof(T));
            return values[RandomSeed.Next(exceptZero ? 1 : 0, values.Length)];
        }

        /// <summary>
        /// Gets the random value from Enum's first part.
        /// Example: if enum has 20 values, this method will return random value from 0-10
        /// It can be used when business logic uses Enum Range, specified by the same enum.
        /// For instance, Business logic has LowerAccessClass and UpperAccessClass as "AccessGroupEnum".
        /// This method can provide value for LowerAccessClass, by <![CDATA[RandomData.GetEnumLowestPart<AccessGroupEnum>(true)]]>
        /// </summary>
        /// <typeparam name="T">Specified enumeration class</typeparam>
        /// <param name="exceptZero">If set to <c>true</c> - returns enum values starting from second (not zero value, which might be NA/Not Use). Default = false</param>
        /// <returns>One enum value from lowest part of whole enum</returns>
        public static T GetEnumLowestPart<T>(bool exceptZero = false)
        {
            T[] values = (T[])Enum.GetValues(typeof(T));
            var middle = Convert.ToInt32(Math.Floor((decimal)values.Length / 2));
            return values[RandomSeed.Next(exceptZero ? 1 : 0, middle)];
        }

        /// <summary>
        /// Gets the random value from Enum's upper part.
        /// Example: if enum has 20 values, one random from 11-20 will be returned
        /// It can be used when business logic uses Enum Range, specified by the same enum.
        /// For instance, Business logic has LowerAccessClass and UpperAccessClass as "AccessGroupEnum".
        /// This method can provide value for UpperAccessClass, by <![CDATA[RandomData.GetEnumUpperPart<AccessGroupEnum>(true)]]>
        /// </summary>
        /// <typeparam name="T">Specified enumeration class</typeparam>
        /// <returns>One enum value from upper part of whole enum</returns>
        public static T GetEnumUpperPart<T>()
        {
            T[] values = (T[])Enum.GetValues(typeof(T));
            var middle = Convert.ToInt32(Math.Floor((decimal)values.Length / 2)) + 1;
            return values[RandomSeed.Next(middle, values.Length)];
        }

        /// <summary>
        /// Picks a random from one of supplied parameters.
        /// </summary>
        /// <typeparam name="T">Type of supplied parameters</typeparam>
        /// <param name="list">The list of parameters (any number)</param>
        /// <returns>One of supplied parameters</returns>
        public static T GetParameter<T>(params T[] list)
        {
            return list[RandomSeed.Next(0, list.Length - 1)];
        }

        #endregion

        #region [Random Objects]
        /// <summary>
        /// Gets the random Drawing.Color value
        /// </summary>
        /// <returns>Randomly chosen Color struct value</returns>
        public static Color GetColor()
        {
            return Color.FromArgb(255, RandomSeed.Next(0, 255), RandomSeed.Next(0, 255), RandomSeed.Next(0, 255));
        }
        #endregion

        #region [Specific randoms]

        /// <summary>
        /// Generates random e-mail address in correct format
        /// </summary>
        /// <returns>Valid random e-mail address</returns>
        public static string GetEmailAddress()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}.{1}@{2}.{3}", GetString(4, 14), GetString(4, 14), GetString(3, 24), GetString(2, 2, StringIncludes.Lowercase));
        }

        /// <summary>
        /// Generates valid IPv4 Adress for tests
        /// </summary>
        /// <returns>Valid IPv4 adress</returns>
        public static string GetIPv4Address()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}.{3}", GetInteger(1, 255), GetInteger(1, 255), GetInteger(1, 255), GetInteger(1, 255));
        }

        /// <summary>
        /// Generates the IPv6 address.
        /// </summary>
        /// <returns>Correct random IP v6 address</returns>
        public static string GetIPv6Address()
        {
            int[] groupsNumeric = new int[8];

            for (int i = 0; i < groupsNumeric.Length; i++)
            {
                groupsNumeric[i] = GetInteger(0, 65535);
            }

            bool middleGroupsAreZero = (groupsNumeric[1] | groupsNumeric[2] | groupsNumeric[3] | groupsNumeric[4] | groupsNumeric[5] | groupsNumeric[6]) == 0;
            bool isValidAdress = (!middleGroupsAreZero) || (groupsNumeric[0] != 0) || ((groupsNumeric[7] != 0) && (groupsNumeric[7] != 1));

            if (!isValidAdress)
            {
                while (groupsNumeric[3] == 0)
                {
                    groupsNumeric[3] = GetInteger(0, 65535);
                }
            }

            string res = String.Format(
                CultureInfo.InvariantCulture,
                "{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}",
                groupsNumeric[0].ToString("X4", CultureInfo.InvariantCulture),
                groupsNumeric[1].ToString("X4", CultureInfo.InvariantCulture),
                groupsNumeric[2].ToString("X4", CultureInfo.InvariantCulture),
                groupsNumeric[3].ToString("X4", CultureInfo.InvariantCulture),
                groupsNumeric[4].ToString("X4", CultureInfo.InvariantCulture),
                groupsNumeric[5].ToString("X4", CultureInfo.InvariantCulture),
                groupsNumeric[6].ToString("X4", CultureInfo.InvariantCulture),
                groupsNumeric[7].ToString("X4", CultureInfo.InvariantCulture));

            return res;
        }

        /// <summary>
        /// Returns prefilled VievModel (POCO object), where
        /// all its properties are random numbers.
        /// String has two words with locale characters and punctuation marks.
        /// </summary>
        public static DummyViewModel GetViewModel()
        {
            DummyViewModel viewModel = new DummyViewModel();

            viewModel.BoolProperty = RandomData.GetBool();
            viewModel.ByteProperty = (byte)RandomData.GetInteger(byte.MinValue, byte.MaxValue);
            viewModel.ShortProperty = (short)RandomData.GetInteger(short.MinValue, short.MaxValue);
            viewModel.IntProperty = RandomData.GetInteger(int.MinValue, int.MaxValue);
            viewModel.DecimalProperty = RandomData.GetDecimal();
            viewModel.FloatProperty = (float)RandomData.GetDecimal();
            viewModel.StringProperty = RandomData.GetStringSentence(2, true, true);
            viewModel.DateTimeProperty = RandomData.GetDateTime();
            viewModel.GuidProperty = Guid.NewGuid();
            viewModel.NullableDateTimeProperty = RandomData.GetDateTime();
            viewModel.TestEnumProperty = RandomData.GetEnum<TestEnum>(true);
            viewModel.NullableTestEnumProperty = null;
            return viewModel;
        }

        /// <summary>
        /// Generates Business ID (Y-tunnus)
        /// </summary>
        /// <returns>Valid Business ID or null</returns>
        public static string GetBusinessIdCodeFinnish()
        {
            string businessId = string.Empty;
            int[] timesAr = new int[] { 7, 9, 10, 5, 8, 4, 2 };
            int midCount = 0;
            for (int i = 0; i <= 6; i++)
            {
                int yNumber = GetInteger(0, 9);
                businessId += yNumber.ToString(CultureInfo.InvariantCulture);
                midCount += yNumber * timesAr[i];
            }

            int checkingNumber = midCount % 11;
            if (checkingNumber == 1) //ei sallita
            {
                return null;
            }

            if ((checkingNumber > 1) && (checkingNumber < 11))
            {
                checkingNumber = 11 - checkingNumber;
            }

            businessId += "-" + checkingNumber.ToString(CultureInfo.InvariantCulture);
            return businessId;
        }

        /// <summary>
        /// Generates Personal Identification Code (henkilötunnuksen)
        /// </summary>
        /// <returns>Valid Personal Identification Code or null</returns>
        public static string GetPersonalIdentificationCodeFinnish()
        {
            DateTime birthdate = GetDateTimeAdultBirthday();
            string randomPart = GetStringNumber(3);
            int modCalcBase = Convert.ToInt32(birthdate.ToString("ddMMyy") + randomPart);
            string checkSum = "0123456789ABCDEFHJKLMNPRSTUVWXY".Substring(modCalcBase % 31, 1);
            string separator = birthdate.Year - 1800 < 100 ? "+" : birthdate.Year - 1900 < 100 ? "-" : "A";

            return String.Format("{0}{1}{2}{3}", birthdate.ToString("ddMMyy"), separator, randomPart, checkSum);
        }
        #endregion

        /// <summary>
        /// Returns Dictionary for use in dropdowns, containing value and display string
        /// format is {"N99", "Text with nationals"}, where N = index, 99 - random number
        /// and Text with nationals is random text 1-3 words and national characters included
        /// </summary>
        public static Dictionary<string, string> GetSelectListItemDictionary(int? listItemCount = null)
        {
            if (listItemCount == null)
            {
                listItemCount = RandomData.GetInteger(1, 20);
            }

            Dictionary<string, string> returnValue = new Dictionary<string, string>();
            for (int i = 1; i <= listItemCount; i++)
            {
                returnValue.Add(
                    string.Concat(i.ToString(CultureInfo.InvariantCulture), RandomData.GetStringNumber(2)),
                    RandomData.GetStringSentence(RandomData.GetInteger(1, 3), false, true));
            }

            return returnValue;
        }

        /// <summary>
        /// Same as <see cref="GetSelectListItemDictionary"/> for values.
        /// This also randomly "selectes" one of values.
        /// </summary>
        /// <param name="listItemCount">The list item count.</param>
        /// <param name="selectedValue">The selected value - replaces randomly generated.</param>
        public static List<SelectListItem> GetSelectListItems(int? listItemCount = null, string selectedValue = null)
        {
            if (listItemCount == null)
            {
                listItemCount = RandomData.GetInteger(1, 20);
            }

            var initialList = GetSelectListItemDictionary(listItemCount);
            int selectedItem = RandomData.GetInteger(1, listItemCount.Value);
            List<SelectListItem> returnList = new List<SelectListItem>();
            int index = 1;
            foreach (KeyValuePair<string, string> keyValuePair in initialList)
            {
                var item = new SelectListItem();
                item.Text = keyValuePair.Value;
                if (index == selectedItem)
                {
                    item.Value = string.IsNullOrEmpty(selectedValue) ? keyValuePair.Key : selectedValue;
                    item.Selected = true;
                }
                else
                {
                    item.Value = keyValuePair.Key;
                    item.Selected = false;
                }

                returnList.Add(item);
                index++;
            }

            return returnList;
        }   
    }
}
