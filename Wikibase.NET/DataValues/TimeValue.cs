using System;
using System.Collections.Generic;
using System.Text;
using MinimalJson;

namespace Wikibase.DataValues
{
    /// <summary>
    /// Data value for times
    /// </summary>
    public class TimeValue : DataValue
    {
        public const string GREGORIAN_CALENDAR = "http://www.wikidata.org/entity/Q1985727";

        public const int PRECISION_Ga = 0; // Gigayear
	    public const int PRECISION_100Ma = 1; // 100 Megayears
	    public const int PRECISION_10Ma = 2; // 10 Megayears
	    public const int PRECISION_Ma = 3; // Megayear
	    public const int PRECISION_100ka = 4; // 100 Kiloyears
	    public const int PRECISION_10ka = 5; // 10 Kiloyears
	    public const int PRECISION_ka = 6; // Kiloyear
	    public const int PRECISION_100a = 7; // 100 years
	    public const int PRECISION_10a = 8; // 10 years
	    public const int PRECISION_YEAR = 9;
	    public const int PRECISION_MONTH = 10;
	    public const int PRECISION_DAY = 11;
	    public const int PRECISION_HOUR = 12;
	    public const int PRECISION_MINUTE = 13;
        public const int PRECISION_SECOND = 14;

        /// <summary>
        /// Point in time, represented per ISO8601
        /// The year always having 11 digits, the date always be signed, in the format +00000002013-01-01T00:00:00Z
        /// </summary>
        public string time { get; set; }

        /// <summary>
        /// Timezone information as an offset from UTC in minutes.
        /// </summary>
        public int timezone { get; set; }

        /// <summary>
        /// If the date is uncertain, how many units before the given time could it be?
        /// The unit is given by the <see cref="precision"/>.
        /// </summary>
        public int before { get; set; }

        /// <summary>
        /// If the date is uncertain, how many units after the given time could it be?
        /// The unit is given by the <see cref="precision"/>.
        /// </summary>
        public int after { get; set; }

        /// <summary>
        /// Unit used for the <see cref="after"/> and <see cref="before"/> values.
        /// </summary>
        public int precision { get; set; }

        /// <summary>
        /// URI identifying the calendar model that should be used to display this time value.
        /// Note that time is always saved in proleptic Gregorian, this URI states how the value should be displayed.
        /// </summary>
        public string calendarmodel { get; set; }

        public TimeValue(string time, int timezone, int before, int after, int precision, string calendarmodel)
        {
            this.time = time;
            this.timezone = timezone;
            this.before = before;
            this.after = after;
            this.precision = precision;
            this.calendarmodel = calendarmodel;
        }

        internal TimeValue(JsonValue value)
        {
            JsonObject obj = value.asObject();
            this.time = obj.get("time").asString();
            this.timezone = obj.get("timezone").asInt();
            this.before = obj.get("before").asInt();
            this.after = obj.get("after").asInt();
            this.precision = obj.get("precision").asInt();
            this.calendarmodel = obj.get("calendarmodel").asString();
        }

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

        public override string getType()
        {
            return "time";
        }
    }
}
