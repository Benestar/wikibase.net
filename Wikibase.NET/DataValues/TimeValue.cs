using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using MinimalJson;

namespace Wikibase.DataValues
{
    /// <summary>
    /// Data value for times
    /// </summary>
    public class TimeValue : DataValue
    {
        /// <summary>
        /// Full wikidata entity path for the (proleptic) Gregorian calendar.
        /// </summary>
        public const string GREGORIAN_CALENDAR = "http://www.wikidata.org/entity/Q1985727";

        // TODO: Change to a enum, and keep the numbers only internally

        /// <summary>
        /// Gigayear, 1 billion years.
        /// </summary>
        public const int PRECISION_Ga = 0; // Gigayear

        /// <summary>
        /// 100 megayears, 100 million years.
        /// </summary>
        public const int PRECISION_100Ma = 1; // 100 Megayears

        /// <summary>
        /// 10 megayears, 10 million years.
        /// </summary>
        public const int PRECISION_10Ma = 2; // 10 Megayears

        /// <summary>
        /// 1 megayear, 1 million years.
        /// </summary>
        public const int PRECISION_Ma = 3; // Megayear

        /// <summary>
        /// 100 kiloyears, 100,000 years.
        /// </summary>
        public const int PRECISION_100ka = 4; // 100 Kiloyears

        /// <summary>
        /// 10 kiloyears, 10,000 years.
        /// </summary>
        public const int PRECISION_10ka = 5; // 10 Kiloyears

        /// <summary>
        /// 1 kiloyear, 1,000 years or one millenium.
        /// </summary>
        public const int PRECISION_ka = 6; // Kiloyear

        /// <summary>
        /// 100 years or one century.
        /// </summary>
        public const int PRECISION_100a = 7; // 100 years

        /// <summary>
        /// 10 years or one decade.
        /// </summary>
        public const int PRECISION_10a = 8; // 10 years

        /// <summary>
        /// 1 year.
        /// </summary>
        public const int PRECISION_YEAR = 9;

        /// <summary>
        /// 1 month.
        /// </summary>
        public const int PRECISION_MONTH = 10;

        /// <summary>
        /// 1 day.
        /// </summary>
        public const int PRECISION_DAY = 11;

        /// <summary>
        /// 1 hour.
        /// </summary>
        public const int PRECISION_HOUR = 12;

        /// <summary>
        /// 1 minute.
        /// </summary>
        public const int PRECISION_MINUTE = 13;

        /// <summary>
        /// 1 second.
        /// </summary>
        public const int PRECISION_SECOND = 14;

        /// <summary>
        /// Point in time, represented per ISO8601
        /// The year always having 11 digits, the date always be signed, in the format +00000002013-01-01T00:00:00Z
        /// </summary>
        public String time
        {
            get;
            set;
        }

        /// <summary>
        /// Timezone information as an offset from UTC in minutes.
        /// </summary>
        public Int32 timezone
        {
            get;
            set;
        }

        /// <summary>
        /// If the date is uncertain, how many units before the given time could it be?
        /// The unit is given by the <see cref="precision"/>.
        /// </summary>
        public Int32 before
        {
            get;
            set;
        }

        /// <summary>
        /// If the date is uncertain, how many units after the given time could it be?
        /// The unit is given by the <see cref="precision"/>.
        /// </summary>
        public Int32 after
        {
            get;
            set;
        }

        /// <summary>
        /// Unit used for the <see cref="after"/> and <see cref="before"/> values.
        /// </summary>
        public Int32 precision
        {
            get;
            set;
        }

        /// <summary>
        /// URI identifying the calendar model that should be used to display this time value.
        /// Note that time is always saved in proleptic Gregorian, this URI states how the value should be displayed.
        /// </summary>
        public String calendarmodel
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new time value with the given settings.
        /// </summary>
        /// <param name="time">Time value in ISO8601 format.</param>
        /// <param name="timezone">Time zone offset in minutes.</param>
        /// <param name="before">Number of <paramref name="precision">units</paramref> the actual time value could be before the given time value.</param>
        /// <param name="after">Number of <paramref name="precision">units</paramref> the actual time value could be after the given time value.</param>
        /// <param name="precision">Date/time precision.</param>
        /// <param name="calendarmodel">Calendar model property.</param>
        public TimeValue(String time, int timezone, int before, int after, int precision, String calendarmodel)
        {
            this.time = time;
            this.timezone = timezone;
            this.before = before;
            this.after = after;
            this.precision = precision;
            this.calendarmodel = calendarmodel;
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
            this.time = obj.get("time").asString();
            this.timezone = obj.get("timezone").asInt();
            this.before = obj.get("before").asInt();
            this.after = obj.get("after").asInt();
            this.precision = obj.get("precision").asInt();
            this.calendarmodel = obj.get("calendarmodel").asString();
        }

        /// <summary>
        /// Encodes as a <see cref="JsonValue"/>.
        /// </summary>
        /// <returns>Encoded class.</returns>
        internal override JsonValue encode()
        {
            return new JsonObject()
                .add("time", time)
                .add("timezone", timezone)
                .add("before", before)
                .add("after", after)
                .add("precision", precision)
                .add("calendarmodel", calendarmodel);
        }

        /// <summary>
        /// Gets the data type identifier.
        /// </summary>
        /// <returns></returns>
        public override string getType()
        {
            return "time";
        }
    }
}