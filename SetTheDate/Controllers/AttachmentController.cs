using SetTheDate.Libraries.Dtos;
using SetTheDate.Libraries.Repositories;
using SetTheDate.ModelFactories;

namespace SetTheDate.Controllers
{
    public class AttachmentController
    {
        private readonly AttachmentModelFactory _attachmentModelFactory;

        public AttachmentController(AttachmentModelFactory attachmentModelFactory)
        {
            _attachmentModelFactory = attachmentModelFactory;
        }

    }
}
