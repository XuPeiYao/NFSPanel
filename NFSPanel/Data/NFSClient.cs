using System.Collections.Generic;
using System.Linq;

namespace NFSPanel.Data {
    public class NFSClient {
        public string Client { get; set; }
        public NFSPermission Permission { get; set; } = NFSPermission.ReadOnly;
        public bool Async { get; set; } = true;
        public NFSSquash Squash { get; set; } = NFSSquash.Root;

        public override string ToString() {
            List<string> options = new List<string>();

            switch (Permission) {
                case NFSPermission.ReadAndWrite:
                    options.Add("rw");
                    break;
                case NFSPermission.ReadOnly:
                    options.Add("ro");
                    break;
            }

            if (Async) {
                options.Add("async");
            } else {
                options.Add("sync");
            }

            switch (Squash) {
                case NFSSquash.All:
                    options.Add("all_squash");
                    break;
                case NFSSquash.Root:
                    options.Add("root_squash");
                    break;
                case NFSSquash.No:
                    options.Add("no_root_squash");
                    break;
            }

            return $"{Client}({string.Join(",", options)})";
        }

        public static NFSClient Parse(string str) {
            var data = str.Split(new char[] { '(', ')' });

            var result = new NFSClient();

            result.Client = data[0];
            if (data.Length > 1) {
                data = data.Skip(1).ToArray();

                if (data[0].Contains("sync")) {
                    result.Async = false;
                }

                if (data[0].Contains("rw")) {
                    result.Permission = NFSPermission.ReadAndWrite;
                }

                if (data[0].Contains("no_root_squash")) {
                    result.Squash = NFSSquash.No;
                } else if (data.Contains("all_squash")) {
                    result.Squash = NFSSquash.All;
                }
            }

            return result;
        }
    }
}