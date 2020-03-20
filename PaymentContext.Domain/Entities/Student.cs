using System.Collections.Generic;
using System.Linq;
using Flunt.Validations;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Entities;

namespace PaymentContext.Domain.Entities
{
    public class Student : Entity
    {
        private IList<Subscription> _subscription;

        public Student(Name name, Document document, Email email)
        {
            Name = name;
            Document = document;
            Email = email;
            _subscription = new List<Subscription>();

            AddNotifications(name, document, email);
        }

        public Name Name { get; private set; }
        public Document Document { get; private set; }
        public Email Email { get; private set; }
        public Address Address { get; private set; }
        public IReadOnlyCollection<Subscription> Subscriptions { get { return _subscription.ToArray(); } }

        public void AddSubscription(Subscription subscription)
        {
            var hasSubscription = false;
            foreach(var sub in _subscription)
            {
                if(sub.Active)
                    hasSubscription = true;
            }

            AddNotifications(new Contract()
                .Requires()
                .IsFalse(hasSubscription, "Student.Subscription", "Você já tem uma assinatura ativa.")
                .AreEquals(0, subscription.Payments.Count, "Student.Subscription.Payments", "Esta assinatura não possui pagamentos.")
            );

            //Alternativa
            // if(hasSubscription)
            //     AddNotification("Student.Subscription", "Você já tem uma assinatura ativa.");
        }
    }
}