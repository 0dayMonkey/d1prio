using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FloorServer.Client.Boss
{
    /// <summary>
    /// The BOss Message class.
    /// </summary>
    public class Bom
    {
        #region Private Fields

        private static readonly List<char> NoDelimitersList = new List<char> { '=', '{', '}' };

        private readonly ILogger<Bom> _logger;

        private readonly Dictionary<string, object> _dict = new Dictionary<string, object>();

        private static readonly long UnixTimeBase = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).ToFileTimeUtc();

        #endregion

        #region Constructors

        protected Bom(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Bom>();
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        internal Bom(ILogger<Bom> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="msg">Bom message</param>
        internal Bom(Bom msg, ILogger<Bom> logger) : this(logger)
        {
            foreach (string tag in msg.Tags)
            {
                _dict[tag] = msg.GetObject(tag);
            }
        }
        #endregion

        /// <summary>
        /// Gets collection of all tags in message object.
        /// </summary>
        public Dictionary<string, object>.KeyCollection Tags
        {
            get { return _dict.Keys; }
        }

        /// <summary>
        /// Gets number of tag/value pairs in Bom.
        /// </summary>
        public int Count
        {
            get { return _dict.Count; }
        }

        /// <summary>
        /// Removes tag and associated value from message object.
        /// </summary>
        /// <param name="tag">Tag to remove.</param>
        /// <returns>true if the element is successfully found and removed; 
        /// otherwise, false. This method returns false if key is not 
        /// found in the Dictionary. </returns>
        /// <exception cref="ArgumentNullException">tag is null</exception>
        public bool Remove(string tag)
        {
            return _dict.Remove(tag);
        }

        /// <summary>
        /// Removes all tags and values from message object.
        /// </summary>
        public void Clear()
        {
            _dict.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="Bom"/> contains the specified tag. 
        /// </summary>
        /// <param name="tag">The key to locate in <see cref="Bom"/></param>
        /// <returns>true if the Dictionary contains an element with the 
        /// specified key; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException" />
        public bool ContainsKey(string tag)
        {
            return _dict.ContainsKey(tag);
        }

        /// <summary>
        /// Checks for value exists in message object.
        /// </summary>
        /// <param name="value">Value object to check.</param>
        /// <returns>true if value exists</returns>
        public bool ContainsValue(object value)
        {
            return _dict.ContainsValue(value);
        }


        #region Parser

        /// <summary>
        /// Parse Tag/Value from string.
        /// </summary>
        /// <param name="msg">Message to parse.</param>
        /// <returns>Parsed message object.</returns>
        internal static Bom Parse(string msg, ILogger<Bom> logger)
        {
            var res = new Bom(logger);
            try
            {
                Parse(msg, 0, res, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error parsing <{0}>", msg), ex);
            }
            return res;
        }

        private static int Parse(string msg, int pos, Bom res, ILogger<Bom> logger)
        {
            try
            {
                while (pos < msg.Length)
                {
                    while (char.IsWhiteSpace(msg[pos]))
                    {
                        pos++;
                    }
                    if (msg[pos].Equals('}'))
                    {
                        return pos + 1;
                    }

                    int kstart = pos;
                    while (!msg[pos].Equals('='))
                    {
                        pos++;
                    }
                    string key = msg.Substring(kstart, pos - kstart);

                    // TODO: check for key length == 0
                    pos++; // skip '='
                    if (msg[pos].Equals('{'))
                    {
                        var values = new List<Bom>();
                        pos = ParseMsgList(msg, pos, values, logger);
                        if (values.Count == 1)
                        {
                            res.PutMsg(key, values[0]);
                        }
                        else
                        {
                            res.PutMsgList(key, values);
                        }
                    }
                    else
                    {
                        var values = new List<string>();
                        pos = ParseStringList(msg, pos, values, logger);
                        if (values.Count == 1)
                        {
                            res.PutString(key, values[0]);
                        }
                        else
                        {
                            res.PutStrList(key, values);
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException ioor)
            {
                logger.LogDebug("+++" + ioor.Message);
            }

            return pos;
        }

        private static int ParseStringList(string msg, int pos, List<string> values, ILogger<Bom> logger)
        {
            try
            {
                char delim = msg[pos];
                pos++;
                int vstart = pos;
                while (!msg[pos].Equals(delim))
                {
                    pos++;
                }
                values.Add(msg.Substring(vstart, pos - vstart));
                pos++;
                if (pos >= msg.Length)
                {
                    return pos;
                }
                if (!(char.IsWhiteSpace(msg[pos]) || msg[pos].Equals('}')))                
                {
                    pos = ParseStringList(msg, pos, values, logger);
                }
            }
            catch (IndexOutOfRangeException ioor)
            {
                logger.LogDebug("++" + ioor.Message);
            }

            return pos;
        }

        private static int ParseMsgList(string msg, int pos, List<Bom> values, ILogger<Bom> logger)
        {
            try
            {
                var b = new Bom(logger);
                pos = Parse(msg, pos + 1, b, logger);
                values.Add(b);
                if (pos >= msg.Length)
                {
                    return pos;
                }
                if (msg[pos].Equals('{'))
                {
                    pos = ParseMsgList(msg, pos, values, logger);
                }
            }
            catch (IndexOutOfRangeException ioor)
            {
                logger.LogDebug("+" + ioor.Message);
            }

            return pos;
        }

        #endregion

        #region Getter / Setter

        /// <summary>
        /// Gets long value from <see cref="Bom"/>.
        /// </summary>
        /// <param name="tag">Tag to get.</param>
        /// <returns>Long value associated with tag or 0L on error.</returns>
        public long GetLong(string tag)
        {
            if (!_dict.ContainsKey(tag)) return 0L;
            try
            {
                return Convert.ToInt64((string)_dict[tag]);
            }
            catch (FormatException fe)
            {
                LogError(tag, fe.Message, _logger);
            }
            catch (OverflowException oe)
            {
                LogError(tag, oe.Message, _logger);
            }
            catch (ArgumentException ae)
            {
                LogError(tag, ae.Message, _logger);
            }
            return 0L;
        }

        /// <summary>
        /// Gets long value with default.
        /// </summary>
        /// <param name="tag">Tag to get.</param>
        /// <param name="def">Default value.</param>
        /// <returns>Long value associated with tag or def.</returns>
        public long GetLong(string tag, long def)
        {
            if (!_dict.ContainsKey(tag))
            {
                return def;
            }
            return GetLong(tag);
        }

        /// <summary>
        /// A .net DateTime object representing the tag value in Local Time <see cref="Bom"/>.
        /// </summary>
        /// <param name="tag">Tag to get.</param>
        /// <returns>DateTiem object, in case of an error date will be 1.1.1970</returns>
        public DateTime GetDateTimeLocal(string tag)
        {
            if (!_dict.ContainsKey(tag)) return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
            try
            {
                return DateTime.FromFileTimeUtc(Convert.ToInt64((string)_dict[tag]) * 10000000 + UnixTimeBase).ToLocalTime();
            }
            catch (FormatException fe)
            {
                LogError(tag, fe.Message, _logger);
            }
            catch (OverflowException oe)
            {
                LogError(tag, oe.Message, _logger);
            }
            catch (ArgumentException ae)
            {
                LogError(tag, ae.Message, _logger);
            }
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
        }

        /// <summary>
        /// Puts long value into <see cref="Bom"/>.
        /// </summary>
        /// <param name="tag">Tag for long value.</param>
        /// <param name="value">Value to set.</param>
        /// <returns><see cref="Bom"/> with long value set.</returns>
        public Bom PutLong(string tag, long value)
        {
            _dict[tag] = value.ToString(CultureInfo.InvariantCulture);
            return this;
        }

        /// <summary>
        /// Gets string from <see cref="Bom"/>.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>String associated with tag or Tags.EMPTY</returns>
        public string GetString(string tag)
        {
            if (!_dict.ContainsKey(tag)) return Boss.Tags.EMPTY;
            try
            {
                return (string)_dict[tag];
            }
            catch (InvalidCastException ice)
            {
                LogError(tag, ice.Message, _logger);
            }
            return Boss.Tags.EMPTY;
        }

        /// <summary>
        /// Gets string with default value.
        /// </summary>
        /// <param name="tag">Tag to get.</param>
        /// <param name="def">Default value.</param>
        /// <returns>String associated with tag or def.</returns>
        public string GetString(string tag, string def)
        {
            if (!_dict.ContainsKey(tag))
                return def;
            return GetString(tag);
        }

        /// <summary>
        /// Puts string value into <see cref="Bom"/>.
        /// </summary>
        /// <param name="tag">Tag to set.</param>
        /// <param name="value">String value to set</param>
        /// <returns>Modified <see cref="Bom"/></returns>
        public Bom PutString(string tag, string value)
        {
            _dict[tag] = value;
            return this;
        }

        /// <summary>
        /// Gets list of string from message.
        /// If value is a single string, a list containing this string is returned.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>List of string values</returns>
        public List<string> GetStrList(string tag)
        {
            if (_dict.ContainsKey(tag))
            {
                try
                {
                    return (List<string>)_dict[tag];
                }
                catch (InvalidCastException)
                {
                    // don't log this is standard behaiviour
                    //log.WarnFormat("Key <{0}>: {1}", tag, ice.Message);
                }
                string tmp = GetString(tag);
                if (!tmp.Equals(Boss.Tags.EMPTY))
                {
                    var res = new List<string> { tmp };
                    return res;
                }

            }
            return new List<string>();
        }

        /// <summary>
        /// Puts list of strings into <see cref="Bom"/>
        /// </summary>
        /// <param name="tag">Tag name for string list.</param>
        /// <param name="values">Strings to add.</param>
        /// <returns>Modified <see cref="Bom"/></returns>
        public Bom PutStrList(string tag, List<string> values)
        {
            _dict[tag] = values;
            return this;
        }

        /// <summary>
        /// Adds string value to stringlist in <see cref="Bom"/>.
        /// Creates new stringlist if tag not exist.
        /// </summary>
        /// <param name="tag">Tag name for string list.</param>
        /// <param name="value">String value to add.</param>
        /// <returns>Modified <see cref="Bom"/></returns>
        /// <exception cref="InvalidCastException">If tag exists and not of type List&lt;string&gt;</exception>
        public Bom AddToStrList(string tag, string value)
        {
            if (_dict.ContainsKey(tag))
                ((List<string>)_dict[tag]).Add(value);
            else
            {
                var l = new List<string> { value };
                _dict[tag] = l;
            }
            return this;
        }

        /// <summary>
        /// Converts dictionary value to double
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public double GetDouble(string key)
        {
            if (!_dict.ContainsKey(key)) return 0.0D;
            try
            {
                return Convert.ToDouble((string)_dict[key]);
            }
            catch (FormatException fe)
            {
                LogError(key, fe.Message, _logger);
            }
            catch (OverflowException oe)
            {
                LogError(key, oe.Message, _logger);
            }
            catch (ArgumentException ae)
            {
                LogError(key, ae.Message, _logger);
            }
            catch (InvalidCastException ice)
            {
                LogError(key, ice.Message, _logger);
            }
            return 0.0D;
        }

        /// <summary>
        /// Converts dictionary value to double or returns specified default
        /// </summary>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public double GetDouble(string key, double def)
        {
            if (!_dict.ContainsKey(key)) return def;

            return GetDouble(key);
        }

        /// <summary>
        /// Sets a double value in the dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Bom PutDouble(string key, double value)
        {
            _dict[key] = value.ToString(CultureInfo.InvariantCulture);
            return this;
        }

        /// <summary>
        /// Gets submessage identified by <paramref name="tag"/> from message object.
        /// Returns NULL if tag not exists.
        /// </summary>
        /// <param name="tag">Tag name for submessage.</param>
        /// <returns>submessage of NULL if <paramref name="tag"/> does not exist.</returns>
        public Bom GetMsg(string tag)
        {
            if (!_dict.ContainsKey(tag)) return null;
            try
            {
                return (Bom)_dict[tag];
            }
            catch (InvalidCastException ice)
            {
                LogError(tag, ice.Message, _logger);
            }
            return null;
        }

        /// <summary>
        /// Gets submessage identified by <paramref name="tag"/> from message object.
        /// Returns default value if tag not exists.
        /// </summary>
        /// <param name="tag">Tag name for submessage.</param>
        /// <param name="def">Default value</param>
        /// <returns><paramref name="def"/> if <paramref name="tag"/> does not exist.</returns>
        public Bom GetMsg(string tag, Bom def)
        {
            if (!_dict.ContainsKey(tag)) return def;

            return GetMsg(tag);
        }


        /// <summary>
        /// Puts submessage into <see cref="Bom"/>.
        /// </summary>
        /// <param name="tag">Tag name for submessage.</param>
        /// <param name="value">submessage</param>
        /// <returns></returns>
        public Bom PutMsg(string tag, Bom value)
        {
            _dict[tag] = value;
            return this;
        }

        /// <summary>
        /// Get lists of Bom messages for the tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public List<Bom> GetMsgList(string tag)
        {
            if (_dict.ContainsKey(tag))
            {
                try
                {
                    return (List<Bom>)_dict[tag];
                }
                catch (InvalidCastException ice)
                {
                    // don't log this is standard behaiviour
                    _logger.LogWarning("Key <{0}>: {1}", tag, ice.Message);
                }
                Bom tmp = GetMsg(tag);
                if (tmp != null)
                {
                    List<Bom> res = new List<Bom>();
                    res.Add(tmp);
                    return res;
                }
            }
            return new List<Bom>();
        }

        /// <summary>
        /// Stores the Bom messages in the dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public Bom PutMsgList(string key, List<Bom> values)
        {
            _dict[key] = values;
            return this;
        }

        private object GetObject(string key)
        {
            if (!_dict.ContainsKey(key))
            {
                return null;
            }

            return _dict[key];
        }

        #endregion

        #region Tools

        private static void LogError(string key, string msg, ILogger<Bom> logger)
        {
            logger.LogError("Key <{0}>: {1}", key, msg);
        }

        /// <summary>
        /// Build string representation of Bom.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var res = new StringBuilder();
            if (_dict.Count == 0) return "";
            foreach (KeyValuePair<string, object> pair in _dict)
            {
                res.AppendFormat(" {0}=", pair.Key);
                if (pair.Value is string)
                {
                    char delim = WhichDelimiter((string)pair.Value, _logger);
                    res.AppendFormat("{0}{1}{0}", delim, pair.Value);
                }
                else if (pair.Value is List<string>)
                {
                    var sb = new StringBuilder();
                    foreach (string s in (List<string>)pair.Value)
                    {
                        sb.Append(s);
                    }
                    char delim = WhichDelimiter(sb.ToString(), _logger);
                    foreach (string s in (List<string>)pair.Value)
                    {
                        res.AppendFormat("{0}{1}{0}", delim, s);
                    }
                }
                else if (pair.Value is Bom)
                {
                    res.AppendFormat("{{{0}}}", pair.Value);
                }
                else if (pair.Value is List<Bom>)
                {
                    // Bom array
                    foreach (Bom b in (List<Bom>)pair.Value)
                    {
                        res.AppendFormat("{{{0}}}", b);
                    }
                }
                else
                {
                    string msg = string.Format("Value of type {0} not supported.", pair.Value.GetType());
                    _logger.LogError(msg);
                    throw new NotSupportedException(msg);
                }
            }

            return res.ToString(1, res.Length - 1);
        }

        /// <summary>
        /// Gets valid delimit char for value.
        /// Char is out of ascii 34 -- 255 excluding chars in <see cref="NoDelimitersList"/>
        /// </summary>
        /// <param name="value">String to get delimiter for.</param>
        /// <returns>Delimiter</returns>
        private static char WhichDelimiter(string value, ILogger<Bom> logger)
        {
            var c = (char)0;
            for (int i = 34; i < 256; i++)
            {
                c = Convert.ToChar(i);
                if (NoDelimitersList.Contains(c))
                {
                    continue;
                }
                if (!value.Contains(c.ToString(CultureInfo.InvariantCulture)))
                {
                    break;
                }
            }
            if (c == 0)
            {
                string msg = string.Format("No delimiter found for <{0}>", value);
                logger.LogError(msg);
                throw new ArgumentException(msg);
            }
            return c;
        }

        #endregion
    }
}