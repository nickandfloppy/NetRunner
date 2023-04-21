using System.IO;

namespace HBot.Util {
    public static class ResourceManager {
        public static string GetResourcePath(string name, ResourceType type = ResourceType.MiscData) {
            string fileName = name;
            switch (type) {
                case ResourceType.JsonData:
                case ResourceType.Config:
                    fileName += ".json";
                    break;
            }

            string path = fileName;
            switch (type) {
                case ResourceType.JsonData:
                case ResourceType.MiscData:
                    path = Path.Combine("Data", path);
                    break;
                case ResourceType.Resource:
                    path = Path.Combine("Resources", path);
                    break;
            }

            return path;
        }

        public static bool ResourceExists(string name, ResourceType type = ResourceType.MiscData) {
            return File.Exists(GetResourcePath(name, type));
        }
    }

    public enum ResourceType {
        JsonData, MiscData, Config, Resource
    }
}
