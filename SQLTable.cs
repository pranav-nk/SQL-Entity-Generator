using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQLEntityGenerator.Classes
{
    public class SQLTable
    {
        public string TableName;
        public string Alias;
        public List<string> Columns;

        public SQLTable()
        {
            Columns = new List<string>();
        }
        public string GetSelectColumnsString()
        {
            string statement = string.Empty;
            foreach (var col in Columns)
            {
                statement += TableName + "." + col + ", ";
            }
            statement = statement.Substring(0, statement.Length - 2);
            return statement;
        }
    }
    public class SQLRelation
    {
        public SQLTable LeftTable;
        public SQLTable RightTable;
        public string LeftColumn;
        public string RightColumn;
        public SQLRelation()
        {

        }
        public string GetSelectString(bool addFrom)
        {
            string statement = string.Empty;
            if (addFrom)
            {
                statement = " FROM " + LeftTable.TableName;
            }
            statement += " INNER JOIN " + RightTable.TableName + " ON " + LeftTable.TableName + "." + LeftColumn + " = " + RightTable.TableName + "." + RightColumn;
            return statement;
        }
    }
    public enum SQLConditionType
    {
        Equals,
        In,
        NotEquals,
        GreaterThan,
        LessThan,
        Contains
    }
    public class SQLCondition
    {
        public SQLConditionType Type;
        public string ColumnName;
        public object Value;
        public SQLCondition()
        {
            Type = SQLConditionType.Equals;
        }

    }
    public class SQLView
    {
        public List<SQLTable> Tables;
        public List<SQLRelation> Relationships;

        public SQLView()
        {
            Tables = new List<SQLTable>();
            Relationships = new List<SQLRelation>();
        }
        public string GenerateSelectStatement()
        {
            string statement = string.Empty;
            string fromString = string.Empty;
            statement += "SELECT ";
            foreach (var table in Tables)
            {
                statement += table.GetSelectColumnsString() + ", ";
            }
            statement = statement.Substring(0, statement.Length - 2);
            bool fromAdded = false;
            foreach (var relation in Relationships)
            {
                if (!fromAdded )
                {
                    fromString += relation.GetSelectString(true);
                    fromAdded = true;
                }
                else
                {
                    fromString += relation.GetSelectString(false);
                }
                
            }
            statement += fromString;
            return statement;
        }
    }
}