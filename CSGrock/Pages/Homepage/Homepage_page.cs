﻿namespace CSGrock.Pages.Homepage
{
    public class Homepage_page
    {
        public static string GetHomepage()
        {
            return $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n\r\n<head>\r\n    <meta charset=\"UTF-8\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />\r\n    <title>Document</title>\r\n    <link rel=\"stylesheet\" href=\"style.css\" />\r\n    <script src=\"script.js\" defer></script>\r\n</head>\r\n\r\n<body>\r\n    <div class=\"header\">\r\n        <div class=\"navbar\">\r\n            <div class=\"logo\">\r\n                <h1>C# Grok</h1>\r\n            </div>\r\n            <div class=\"menu\">\r\n                <ul>\r\n                    <li><a href=\"#\">Home</a></li>\r\n                    <li><a onclick=\"setElementFocus('right')\">Download</a></li>\r\n                    <li><a onclick=\"setElementFocus('ex-img')\">How it works</a></li>\r\n                    <li><a href=\"#\">Github</a></li>\r\n                </ul>\r\n            </div>\r\n        </div>\r\n    </div>\r\n    <div class=\"body\">\r\n        <div class=\"top-holder\">\r\n            <div class=\"split centered\">\r\n                <div class=\"left info-text\">\r\n                    <h2>Public URLs for Localhost</h2>\r\n                    <h4>\r\n                        Just share your localhost website to the public with a single command.\r\n                    </h4>\r\n                    <p>Create HTTP tunnels on your windows machine and share them with your friends or clients.</p>\r\n                </div>\r\n                <div class=\"right\">\r\n                    <div class=\"framework-select\">\r\n                        <label for=\"framework-select\">Select the framework your using</label>\r\n                        <select name=\"\" id=\"framework-select\">\r\n                            <option value=\"None\">None</option>\r\n                            <option value=\"Laravel\">Laravel (PHP)</option>\r\n                            <option value=\"Djnago\">Djnago (Python)</option>\r\n                            <option value=\"Flask\">Flask (Python)</option>\r\n                            <option value=\"Ruby\">Ruby on Rails</option>\r\n                            <option value=\"ASP.NET\">ASP.NET (C#, Visual Basic)</option>\r\n                            <option value=\"Spring\">Spring Boot (Java)</option>\r\n                            <option value=\"Hugo\">Hugo (Go)</option>\r\n                            <option value=\"Apache / Ngnix\">Apache / Ngnix</option>\r\n                            <option value=\"Next.js\">Next.js</option>\r\n                            <option value=\"React\">React</option>\r\n                        </select>\r\n                    </div>\r\n                    <p id=\"framework-info-text\">Hugo development servers run on port 1313 by default</p>\r\n                    <div class=\"port-select\">\r\n                        <label for=\"port-select\">Or select your own local port</label>\r\n                        <input type=\"number\" id=\"port-select\" placeholder=\"Port\" />\r\n                    </div>\r\n                    <div class=\"copy-cmd\">\r\n                        <label for=\"copy-cmd\">Paste this in your terminal to start a tunnel</label>\r\n                        <input type=\"text\" id=\"copy-cmd\" readonly value=\"csgrok http 1300\">\r\n                    </div>\r\n                    <div class=\"download\">\r\n                        <label for=\"download-button\">Havent installed yet? Download now</label>\r\n                        <div class=\"download-button-holder\">\r\n                            <button id=\"download-button\">Download</button>\r\n                            <select name=\"\" id=\"select-platform\">\r\n                                <option value=\"\">Windows</option>\r\n                                <option disabled=\"true\">Linux (soon)</option>\r\n                                <option disabled=\"true\">Mac (soon)</option>\r\n                            </select>\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n        <div class=\"explanation-image-holder\">\r\n            <img class=\"ex-img\" id=\"explanation-image\" width=\"80%\" />\r\n        </div>\r\n    </div>\r\n</body>\r\n\r\n</html>";
        }
    }
}
