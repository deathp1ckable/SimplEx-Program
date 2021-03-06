﻿using SimplExModel.Model;
using SimplExModel.Model.Inherited;
using System;

namespace SimplExServer.Builders
{
    public abstract class MarkSystemBuilder : Builder<MarkSystem>
    {
        public string Description { get; set; }
        public ExamBuilder ParentExamBuilder { get; private set; }

        protected MarkSystemBuilder(ExamBuilder examBuilder) 
        {
            if (examBuilder == null)
                throw new ArgumentNullException(nameof(examBuilder));
            ParentExamBuilder = examBuilder;
        }

        public static MarkSystemBuilder CreateBuilder(MarkSystem markSystem, ExamBuilder examBuilder)
        {
            if (markSystem == null)
                throw new ArgumentNullException(nameof(markSystem));
            if (markSystem is FiveStepMarkSystem fiveStepMarkSystem)
                return new FiveStepMarkSystemBuilder(fiveStepMarkSystem, examBuilder);
            else
                return null;
        }

        public override void Load(MarkSystem instance)
        {
            base.Load(instance);
            Description = Instance.Description;
        }
        public override MarkSystem GetBuildedInstance()
        {
            Instance.Description = Description;
            return Instance;
        }
    }
}
