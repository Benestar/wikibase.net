using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using MinimalJson;

namespace Wikibase.DataValues
{
    /// <summary>
    /// The precision for a <see cref="TimeValue"/>.
    /// </summary>
    public enum TimeValuePrecision
    {
        /// <summary>
        /// Gigayear, 1 billion years.
        /// </summary>
        GigaYear = 0,

        /// <summary>
        /// 100 megayears, 100 million years.
        /// </summary>
        HundredMegaYears = 1,

        /// <summary>
        /// 10 megayears, 10 million years.
        /// </summary>
        TenMegaYears = 2,

        /// <summary>
        /// 1 megayear, 1 million years.
        /// </summary>
        MegaYear = 3,

        /// <summary>
        /// 100 kiloyears, 100,000 years.
        /// </summary>
        HundredKiloYears = 4,

        /// <summary>
        /// 10 kiloyears, 10,000 years.
        /// </summary>
        TenKiloYears = 5,

        /// <summary>
        /// 1 kiloyear, 1,000 years or one millenium.
        /// </summary>
        Millenium = 6,

        /// <summary>
        /// 100 years or one century.
        /// </summary>
        Century = 7,

        /// <summary>
        /// 10 years or one decade.
        /// </summary>
        Decade = 8,

        /// <summary>
        /// 1 year.
        /// </summary>
        Year = 9,

        /// <summary>
        /// 1 month.
        /// </summary>
        Month = 10,

        /// <summary>
        /// 1 day.
        /// </summary>
        Day = 11,

        /// <summary>
        /// 1 hour.
        /// </summary>
        Hour = 12,

        /// <summary>
        /// 1 minute.
        /// </summary>
        Minute = 13,

        /// <summary>
        /// 1 second.
        /// </summary>
        Second = 14,
    }

    /// <summary>
    /// The calendar models supported by WikiData.
    /// </summary>
    public enum CalendarModel
    {
        /// <summary>
        /// Undefined calendar model.
        /// </summary>
        Unknown,

        /// <summary>
        /// Gregorian calendar, proleptic if necessary.
        /// </summary>
        GregorianCalendar,

        /// <summary>
        /// Julian Calendar.
        /// </summary>
        JulianCalendar
    }

    /// <summary>
    /// Data value for times
    /// </summary>
    public class TimeValue : DataValue
    {
        #region Json names

        /// <summary>
        /// The identifier of this data type in the serialized json object.
        /// </summary>
        public const String TypeJsonName = "time";

        /// <summary>
        /// The name of the <see cref="DisplayCalendarModel"/> property in the serialized json object.
        /// </summary>
        private const String CalendarModelJsonName = "calendarmodel";

        /// <summary>
        /// The name of the <see cref="FullValue"/> property in the serialized json object.
        /// </summary>
        private const String TimeJsonName = "time";

        /// <summary>
        /// The name of the <see cref="TimeZoneOffset"/> property in the serialized json object.
        /// </summary>
        private const String TimeZoneJsonName = "timezone";

        /// <summary>
        /// The name of the <see cref="Before"/> property in the serialized json object.
        /// </summary>
        private const String BeforeJsonName = "before";

        /// <summary>
        /// The name of the <see cref="After"/> property in the serialized json object.
        /// </summary>
        private const String AfterJsonName = "after";

        /// <summary>
        /// The name of the <see cref="Precision"/> property in the serialized json object.
        /// </summary>
        private const String PrecisionJsonName = "precision";

        #endregion Json names

        #region private fields

        private static Dictionary<CalendarModel, String> _calendarModelIdentifiers = new Dictionary<CalendarModel, String>()
        {
             {CalendarModel.GregorianCalendar, "http://www.wikidata.org/entity/Q1985727" },
             {CalendarModel.JulianCalendar, "http://www.wikidata.org/entity/Q1985786"}
        };

        #endregion private fields

        #region properties

        /// <summary>
        /// Gets or sets the date and time.
        /// </summary>
        /// <value>The date and time.</value>
        public DateTime DateTime
        {
            get
            {
                return GetDateTimeValue();
            }
            set
            {
                FullValue = value.ToString("+0000000YYYY-NN-DDTHH:MM:SSZ", CultureInfo.InvariantCulture);
            }
        }

        private DateTime GetDateTimeValue()
        {
            if ( !FullValue.StartsWith("+0000000", StringComparison.Ordinal) )
                throw new InvalidOperationException("Time value out of range");

            return DateTime.Parse(FullValue.Substring(8), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Point in time, represented per ISO8601
        /// The year always having 11 digits, the date always be signed, in the format +00000002013-01-01T00:00:00Z
        /// </summary>
        public String FullValue
        {
            get;
            set;
        }

        /// <summary>
        /// Timezone information as an offset from UTC in minutes.
        /// </summary>
        public Int32 TimeZoneOffset
        {
            get;
            set;
        }

        /// <summary>
        /// If the date is uncertain, how many units before the given time could it be?
        /// The unit is given by the <see cref="Precision"/>.
        /// </summary>
        public Int32 Before
        {
            get;
            set;
        }

        /// <summary>
        /// If the date is uncertain, how many units after the given time could it be?
        /// The unit is given by the <see cref="Precision"/>.
        /// </summary>
        public Int32 After
        {
            get;
            set;
        }

        /// <summary>
        /// Unit used for the <see cref="After"/> and <see cref="Before"/> values.
        /// </summary>
        public TimeValuePrecision Precision
        {
            get;
            set;
        }

        /// <summary>
        /// Calendar model that should be used to display this time value.
        /// </summary>
        /// <remarks>
        /// Note that time is always saved in proleptic Gregorian, this URI states how the value should be displayed.
        /// </remarks>
        public CalendarModel DisplayCalendarModel
        {
            get;
            set;
        }

        #endregion properties

        #region constructor

        /// <summary>
        /// Creates a new time value with the given settings.
        /// </summary>
        /// <param name="time">Time value in ISO8601 format (with 11 year digits).</param>
        /// <param name="timeZoneOffset">Time zone offset in minutes.</param>
        /// <param name="before">Number of <paramref name="precision">units</paramref> the actual time value could be before the given time value.</param>
        /// <param name="after">Number of <paramref name="precision">units</paramref> the actual time value could be after the given time value.</param>
        /// <param name="precision">Date/time precision.</param>
        /// <param name="calendarModel">Calendar model property.</param>
        public TimeValue(String time, Int32 timeZoneOffset, Int32 before, Int32 after, TimeValuePrecision precision, CalendarModel calendarModel)
        {
            this.FullValue = time;
            this.TimeZoneOffset = timeZoneOffset;
            this.Before = before;
            this.After = after;
            this.Precision = precision;
            this.DisplayCalendarModel = calendarModel;
        }

        /// <summary>
        /// Creates a new time value with the given settings.
        /// </summary>
        /// <param name="time">Time value in ISO8601 format (with 11 year digits).</param>
        /// <param name="timeZoneOffset">Time zone offset in minutes.</param>
        /// <param name="before">Number of <paramref name="precision">units</paramref> the actual time value could be before the given time value.</param>
        /// <param name="after">Number of <paramref name="precision">units</paramref> the actual time value could be after the given time value.</param>
        /// <param name="precision">Date/time precision.</param>
        /// <param name="calendarModel">Calendar model property.</param>
        public static TimeValue DateValue(DateTime time)
        {
            return new TimeValue(
                time.ToString("+0000000YYYY-NN-DDTHH:MM:SSZ", CultureInfo.InvariantCulture),
                0,  // timezoneoffset
                0,  // before
                0,  // after
                TimeValuePrecision.Day,
                CalendarModel.GregorianCalendar);
        }

        /// <summary>
        /// Parses a <see cref="JsonValue"/> to a time value.
        /// </summary>
        /// <param name="value"><see cref="JsonValue"/> to parse.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        internal TimeValue(JsonValue value)
        {
            if ( value == null )
                throw new ArgumentNullException("value");

            JsonObject obj = value.asObject();
            this.FullValue = obj.get(TimeJsonName).asString();
            this.TimeZoneOffset = obj.get(TimeZoneJsonName).asInt();
            this.Before = obj.get(BeforeJsonName).asInt();
            this.After = obj.get(AfterJsonName).asInt();
            this.Precision = (TimeValuePrecision)obj.get(PrecisionJsonName).asInt();
            var calendar = obj.get(CalendarModelJsonName).asString();
            if ( _calendarModelIdentifiers.Any(x => x.Value == calendar) )
            {
                this.DisplayCalendarModel = _calendarModelIdentifiers.First(x => x.Value == calendar).Key;
            }
            else
            {
                this.DisplayCalendarModel = CalendarModel.Unknown;
            }
        }

        #endregion constructor

        #region methods

        /// <summary>
        /// Encodes as a <see cref="JsonValue"/>.
        /// </summary>
        /// <returns>Encoded class.</returns>
        /// <exception cref="InvalidOperationException"><see cref="DisplayCalendarModel"/> is <see cref="CalendarModel.Unknown"/>.</exception>
        internal override JsonValue Encode()
        {
            if ( DisplayCalendarModel == CalendarModel.Unknown )
            {
                throw new InvalidOperationException("Calendar model value not set.");
            }

            return new JsonObject()
                .add(TimeJsonName, FullValue)
                .add(TimeZoneJsonName, TimeZoneOffset)
                .add(BeforeJsonName, Before)
                .add(AfterJsonName, After)
                .add(PrecisionJsonName, Convert.ToInt32(Precision))
                .add(CalendarModelJsonName, _calendarModelIdentifiers[DisplayCalendarModel]);
        }

        /// <summary>
        /// Gets the data type identifier.
        /// </summary>
        /// <returns></returns>
        protected override string JsonName()
        {
            return TypeJsonName;
        }

        #endregion methods
    }
}