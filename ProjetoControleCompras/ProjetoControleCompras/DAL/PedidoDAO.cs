﻿using ProjetoControleCompras.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoControleCompras.DAL
{
    class PedidoDAO
    {
        private PedidoDAO() { }

        private static Context ctx = SingletonContext.GetInstance();

        public static bool CadastrarPedido(Pedido pedido)
        {
            ctx.Pedidos.Add(pedido);
            ctx.SaveChanges();
            return true;
        }

        public static bool AtualizarStatusPedido(Pedido pedido)
        {
            Pedido p = ctx.Pedidos.Find(pedido.IdPedido);
            if (p != null)
            {
                ctx.Entry(p).CurrentValues.SetValues(pedido);
                ctx.SaveChanges();
                return true;
            }
            else
                return false;
            
        }

        public static List<Pedido> BuscarPedidoPorAgenteEStatus(Agente solicitante, string status)
        {
            return ctx.Pedidos.Include("ItensPedido").
                Where(x => x.Solicitante.IdAgente.Equals(solicitante.IdAgente) && x.Status.Equals(status)).ToList();
        }

        public static List<Pedido> ListarPedidos()
        {
            return ctx.Pedidos.Include("Solicitante.Cargo").Include("Solicitante.Setor").Include("ItensPedido").ToList();
        }

        public static List<Pedido> ListarPedidosPorStatus(string status)
        {
            return ctx.Pedidos.Include("Solicitante.Cargo").Include("Solicitante.Setor").Include("ItensPedido").
                Where(x => x.Status.Equals(status)).ToList();
        }

        // Metodo para Listar os Pedidos de um Setor que não estão com Status 0 (Ag Validacao Gestor)
        public static List<Pedido> ListarPedidosPorSetorEStatusDiff(int idSetor, string status)
        {
            return ctx.Pedidos.Include("Solicitante.Cargo").Include("Solicitante.Setor").Include("ItensPedido").
                Where(x => x.Solicitante.Setor.IdSetor.Equals(idSetor)).Where(x => x.Status != status).ToList();
        }

        public static List<Pedido> ListarPedidosPorSetorEStatusIgual(int idSetor, string status)
        {
            return ctx.Pedidos.Include("Solicitante.Cargo").Include("Solicitante.Setor").Include("ItensPedido").
                Where(x => x.Solicitante.Setor.IdSetor.Equals(idSetor)).Where(x => x.Status.Equals(status)).ToList();
        }

        public static List<Pedido> ListarPedidosPorStatusIgual(string status1, string status2)
        {
            return ctx.Pedidos.Include("Solicitante.Cargo").Include("Solicitante.Setor").Include("ItensPedido").
                Where(x => x.Status.Equals(status1)).Where(x => x.Status.Equals(status2)).ToList();
        }
        public static List<Pedido> ListarPedidosPorStatusDif(string status1, string status2)
        {
            return ctx.Pedidos.Include("Solicitante.Cargo").Include("Solicitante.Setor").Include("ItensPedido").
                Where(x => x.Status != status1).Where(x => x.Status != status2).ToList();
        }
    }
}
