﻿using SimplExDb.Database;
using SimplExModel.Model;
using SimplExModel.Model.Data;

namespace SimplExServer.Service.DatabaseHandling.Maps
{
    class QuestionMap : ClassMap<QuestionData>
    {
        protected override void MapClass()
        {
            MapId("QuestionID");
            Map("QuestionNumber", () => Instance.QuestionNumber, (a) => Instance.QuestionNumber = a);
            Map("Content", () => Instance.Content, (a) => Instance.Content = a);
            Map("QuestionType", () => Instance.QuestionTypeName, (a) => Instance.QuestionTypeName = a);
            Map("Points", () => Instance.Points, (a) => Instance.Points = a);
            Foreign<Theme>("ThemeID");
            Foreign<QuestionGroup>("QuestionGroupID");
            Table("questions");
        }
    }
}
