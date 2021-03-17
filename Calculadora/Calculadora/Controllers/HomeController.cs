using Calculadora.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Calculadora.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet] // facultativa
        public IActionResult Index()
        {
            //prepara os valores iniciais da View
            ViewBag.Visor = "0";
            ViewBag.PrimeiroOperador = "Sim";
            ViewBag.Operador = "";
            ViewBag.PrimeiroOperando = "";
            ViewBag.LimpaVisor = "Sim";

            return View();
        }

        [HttpPost]
        public IActionResult Index(string bt, string visor, string primeiroOperador, string primeiroOperando, string operador, string limpaVisor)
        {    
            // avaliar o valor associado à variável 'botao'
            switch (bt) {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "0":
                    //atribuir ao 'visor' o valor do botao
                    if (visor == "0" || limpaVisor == "Sim") 
                        visor = bt;
                    else                       
                        visor = visor + bt;
                    limpaVisor = "Nao";
                    break;

                case "+/-":
                    // faz a inversão do valor do visor
                    if (visor.StartsWith('-'))
                        visor = visor.Substring(1);
                    else
                        visor = "-" + visor;
                    break;

                case ",":
                    // faz a gestão da parte decimal
                    if (!visor.Contains(','))
                        visor = visor + ",";
                    break;

                case "+":
                case "-":
                case "x":
                case "/":
                case "=":
                    limpaVisor = "Sim"; // marcar o visor como sendo necessário o seu reinicio
                    if (primeiroOperador != "Sim") { 
                    
                        //esta é a 2ª vez (ou mais) que se selecionou um 'operador'
                        //efetuar a operação com o operador antigo, e os valores dos operandos
                        double operando1 = Convert.ToDouble(primeiroOperando);
                        double operando2 = Convert.ToDouble(visor);
                        //efetuar a operação aritmética
                    
                    switch (operador)
                        {
                            case "+":
                                visor = operando1 + operando2 + "";
                                break;
                            case "-":
                                visor = operando1 - operando2 + "";
                                break;
                            case "x":
                                visor = operando1 * operando2 + "";
                                break;
                            case "/":
                                visor = operando1 / operando2 + "";
                                break;

                        }
                    }
                    // armazenar os valores atuais para calculos futuros
                    // visor atual, que passa a '1º operando'
                    primeiroOperando = visor;
                    // guardar o valor do operador
                    operador = bt;
                    if ( bt != "=")
                        // assinalar que já se escolheu um operador
                        primeiroOperador = "Nao";
                    else
                        // assinalar que já se escolheu um operador
                        primeiroOperador = "Sim";
                    break;

                case "C":
                    
                    visor = "0";
                    primeiroOperador = "";
                    operador = "";
                    primeiroOperando = "";
                    limpaVisor = "Sim";
                    break;


            } // fim do switch

            // enviar o valor do 'visor' para a view
            ViewBag.Visor = visor;
            // preciso de manter o 'estado' das vars. auxiliares
            ViewBag.PrimeiroOperador = primeiroOperador;
            ViewBag.Operador = operador;
            ViewBag.PrimeiroOperando = primeiroOperando;
            ViewBag.LimpaVisor = limpaVisor;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
