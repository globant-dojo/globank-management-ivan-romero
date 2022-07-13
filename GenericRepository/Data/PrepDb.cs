using GenericRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace GenericRepository.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool migrateDb, bool populatedb)
        {
            using (IServiceScope? serviceScope = app.ApplicationServices.CreateScope())
            {                
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), migrateDb, populatedb);
            }
        }
        private static void SeedData(AppDbContext? context, bool migrateDb, bool populatedb)
        {
            if (migrateDb)
            {
                Console.WriteLine("--> Intentando aplicar la migración...");
                try
                {
                    context?.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> No se pudo ejecutar la migración: {ex.Message}");
                }
            }

            if (context == null)
                return;

            if (populatedb && !context.Clientes.Any())
            {
                Console.WriteLine("--> Cargando data en BD...");

                context.Clientes.AddRange(
                    new Cliente
                    {
                        Nombre = "Carlos García",
                        Genero = "Masculino",
                        Edad = 20,
                        Identificacion = "175489123",
                        Direccion = "Quito",
                        Telefono = "0987512455",
                        Contrasena = "123",
                        Cuentas = {
                            new Cuenta
                            {
                                NumeroCuenta = "751891", SaldoInicial = 100.15M, TipoCuenta = "Ahorros",
                                Movimientos =
                                {
                                    new Movimiento
                                    {
                                        TipoMovimiento = "C",
                                        Valor = 20.12M,
                                        Saldo = 120.27M
                                    },
                                    new Movimiento
                                    {
                                        TipoMovimiento = "D",
                                        Valor = -20.27M,
                                        Saldo = 100M
                                    },
                                }
                            }
                        }
                    },
                    new Cliente
                    {
                        Nombre = "Jose Lema",
                        Direccion = "Otavalo sn y principal",
                        Telefono = "098254785",
                        Contrasena = "1234",
                        Cuentas = {
                            new Cuenta
                            {
                                NumeroCuenta = "478758", SaldoInicial = 2000M, TipoCuenta = "Ahorros",
                                Movimientos =
                                {
                                    new Movimiento
                                    {
                                        Fecha = DateTime.Parse("2022-06-30 15:00:00"),
                                        TipoMovimiento = "D",
                                        Valor = -575M,
                                        Saldo = 1425M
                                    }
                                }
                            },
                            new Cuenta
                            {
                                NumeroCuenta = "585545", SaldoInicial = 1000M, TipoCuenta = "Corriente",
                                Movimientos =
                                {
                                    new Movimiento
                                    {
                                        Fecha = DateTime.Parse("2022-06-30 15:00:00"),
                                        TipoMovimiento = "D",
                                        Valor = -575M,
                                        Saldo = 425M
                                    }
                                }
                            }
                        }
                    },
                    new Cliente
                    {
                        Nombre = "Marianela Montalvo",
                        Direccion = "Amazonas y NNUU",
                        Telefono = "097548965",
                        Contrasena = "5678",
                        Cuentas = {
                            new Cuenta
                            {
                                NumeroCuenta = "225487", SaldoInicial = 100M, TipoCuenta = "Corriente",
                                Movimientos =
                                {
                                    new Movimiento
                                    {
                                        Fecha = DateTime.Parse("2022-06-30 15:00:00"),
                                        TipoMovimiento = "C",
                                        Valor = 100M,
                                        Saldo = 200M
                                    }
                                }
                            },
                            new Cuenta
                            {
                                NumeroCuenta = "496825", SaldoInicial = 540M, TipoCuenta = "Ahorros",
                                Movimientos =
                                {
                                    new Movimiento
                                    {
                                        Fecha = DateTime.Parse("2022-07-10 15:00:00"),
                                        TipoMovimiento = "D",
                                        Valor = -540M,
                                        Saldo = 0M
                                    }
                                }
                            }
                        }
                    },
                    new Cliente
                    {
                        Nombre = "Juan Osorio",
                        Direccion = "13 junio y Equinoccial",
                        Telefono = "098874587",
                        Contrasena = "1245",
                        Cuentas = {
                            new Cuenta
                            {
                                NumeroCuenta = "495878", SaldoInicial = 0M, TipoCuenta = "Ahorros",
                                Movimientos =
                                {
                                    new Movimiento
                                    {
                                        Fecha = DateTime.Parse("2022-06-30 15:00:00"),
                                        TipoMovimiento = "C",
                                        Valor = 100M,
                                        Saldo = 100M
                                    }
                                }
                            }
                        }
                    }); 

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> Existen registros en la BD");
            }
        }
    }
}