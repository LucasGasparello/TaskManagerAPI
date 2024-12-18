using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace TaskManagerAPI.Models
{
    public class TaskItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }  // Identificador único da tarefa

        public string ProjectId { get; set; }  // Id do projeto ao qual a tarefa pertence

        public string Title { get; set; }  // Título da tarefa

        public string Description { get; set; }  // Descrição da tarefa

        public DateTime DueDate { get; set; }  // Data de vencimento da tarefa

        public EnumStatus Status { get; set; }  // Status da tarefa (Pendente, Em andamento, Concluída)

        public EnumPriority Priority { get; set; }  // Prioridade da tarefa (Baixa, Média, Alta)

        // Histórico de alterações (armazenando ações realizadas na tarefa)
        public List<ChangeHistory> History { get; set; } = new List<ChangeHistory>();

        // Comentários associados à tarefa
        public List<Comment> Comments { get; set; } = new List<Comment>();

        // Data e usuário da criação da tarefa
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime CompletedAt { get; set; }
    }

    // Enum para o status da tarefa
    public enum EnumStatus : int
    {
        PENDING = 0,
        IN_PROGRESS = 1,
        DONE = 2
    }

    // Enum para a prioridade da tarefa
    public enum EnumPriority : int
    {
        LOW = 0,
        MEDIUM = 1,
        HIGH = 2
    }

    // Classe para o histórico de alterações de cada tarefa
    public class ChangeHistory
    {
        public string Action { get; set; }  // Descrição da ação realizada (ex: "Task Created", "Task Updated")
        public DateTime Date { get; set; }  // Data da alteração
        public string ChangedBy { get; set; }  // Usuário que realizou a alteração
        public Dictionary<string, string> Changes { get; set; }  // Dicionário contendo as alterações realizadas
    }

    // Classe para os comentários
    public class Comment
    {
        public string Content { get; set; }  // Conteúdo do comentário
        public DateTime CreatedAt { get; set; }  // Data de criação do comentário
        public string CreatedBy { get; set; }  // Usuário que fez o comentário
    }
}
