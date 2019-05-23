using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NFSPanel.Data {
    public class NFSMount {
        public string Path { get; set; }
        public List<NFSClient> Clients { get; set; } = new List<NFSClient>();

        public override string ToString() {
            return $"{Path} {string.Join(" ", Clients.Where(x => !string.IsNullOrWhiteSpace(x.Client)).Select(x => x.ToString()))}";
        }

        public static NFSMount Parse(string str) {
            var reg = new Regex(@"\s+");
            var data = reg.Split(str, 2);

            var result = new NFSMount();
            result.Path = data[0];
            if (data.Length > 1) {
                result.Clients.AddRange(
                    data[1].Split(new char[] { ')' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim() + ")")
                        .Select(x => NFSClient.Parse(x))
                );
            }

            return result;
        }
    }
}
