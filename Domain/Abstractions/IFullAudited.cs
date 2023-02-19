namespace Domain.Abstractions;

public interface IFullAudited : IAudited, ICreationAudited, IModificationAudited, IDeletionAudited
{
}