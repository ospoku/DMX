using Microsoft.ML.Data;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DMX.Services
{
public class MemoTextModel
    {
        private static string modelPath = "memoModel.zip";

        public class MemoData
        {
            [LoadColumn(0)]
            public string Text { get; set; }
        }

        public class MemoPrediction
        {
            [ColumnName("PredictedLabel")]
            public string Prediction { get; set; }
        }

        public static void TrainModel()
        {
            var mlContext = new MLContext();
            var data = new List<MemoData>
        {
            new MemoData { Text = "Please find attached the report" },
            new MemoData { Text = "Let me know if you have any questions" },
            new MemoData { Text = "We will discuss this in the meeting" },
            new MemoData { Text = "Thank you for your time" }
        };

            var trainData = mlContext.Data.LoadFromEnumerable(data);
            var pipeline = mlContext.Transforms.Text.FeaturizeText("Text")
                .Append(mlContext.Transforms.Conversion.MapValueToKey("Text"))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("Text"));

            var model = pipeline.Fit(trainData);
            mlContext.Model.Save(model, trainData.Schema, modelPath);
        }

        public static string PredictText(string input)
        {
            var mlContext = new MLContext();
            ITransformer trainedModel = mlContext.Model.Load(modelPath, out _);
            var predEngine = mlContext.Model.CreatePredictionEngine<MemoData, MemoPrediction>(trainedModel);

            var result = predEngine.Predict(new MemoData { Text = input });
            return result.Prediction;
        }
    }

}
