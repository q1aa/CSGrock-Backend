namespace CSGrock.Pages._404
{
    public class Get404
    {
        public static string Get404Page()
        {
            return @"<!DOCTYPE html>
                    <html lang=""en"">
                    <head>
                        <meta charset=""UTF-8"">
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                        <title>Document</title>
                        <style>
                            * {
                              margin: 0;
                              padding: 0;
                              box-sizing: border-box;
                            }

                            body {
                              font-family: 'Poppins', sans-serif;
                              background-color: #413f3f;
                            }

                            h1 {
                              font-family: ""Bevan"";
                              font-size: 130px;
                              margin: 10vh 0 0;
                              text-align: center;
                              color: #8abeb7;
                            }

                            p {
                              font-size: 20px;
                              color: #8abeb7;
                              text-align: center;
                              margin: 20px 0;
                            }

                            .key {
                              color: #8abeb7;
                            }

                            .value-false {
                              color: #cc6666;
                            }

                            .value-number {
                              color: #b5bd68;
                            }

                            .value-text {
                              color: #f0c674;
                            }

                            .bracket {
                              color: #b294bb;
                            }
                        </style>
                    </head>
                    <body>
                      <h1>HTTP: <span class=""value-false"">404</span></h1>
                      <p id=""p""></p>
                    
                      <script>
                        let text2 = [
                          `<span class=""bracket"">{ </span>`,
                          `<span class=""key"">""success""</span>: <span class=""value-false"">false</span>,`,
                          `<span class=""key"">""status_code""</span>: <span class=""value-number"">404</span>,`,
                          `<span class=""key"">""message""</span>: <span class=""value-text"">""The request URL was not found on this server.""</span>,`,
                          `<a href=""/""><span class=""key"">""home_page_url""</span>: <span class=""value-text"">""/""</span></a>`,
                          `<span class=""bracket""> }</span>`
                        ];
                        let p = document.getElementById('p');
                        for (let i = 0; i < text2.length; i++) {
                          setTimeout(() => {
                            p.innerHTML += text2[i];
                          }, i * 200);
                        }
                      </script>
                   </body>
                  </html>";
        }
    }
}
