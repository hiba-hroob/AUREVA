
(() => {
    // Keep the demo data in memory only.
    // A full browser refresh resets all changes.

    const state = {
        clients: [
            {
                id: 1,
                fullName: "Sofia Bennett",
                email: "sofia@example.com",
                phone: "555-0101",
                totalVisits: 3,
                totalSpent: 180,
                favoriteService: "Signature Blowout"
            },
            {
                id: 2,
                fullName: "Amelia Rose",
                email: "amelia@example.com",
                phone: "555-0102",
                totalVisits: 2,
                totalSpent: 95,
                favoriteService: "Manicure Ritual"
            }
        ],

        services: [
            {
                id: 1,
                name: "Signature Blowout",
                description: "A polished, salon-quality blowout.",
                price: 35,
                durationMinutes: 45,
                isActive: true
            },
            {
                id: 3,
                name: "Manicure Ritual",
                description: "A refined manicure experience.",
                price: 30,
                durationMinutes: 57,
                isActive: false
            },
            {
                id: 4,
                name: "Manicure Ritual",
                description: "A complete manicure treatment.",
                price: 55,
                durationMinutes: 74,
                isActive: true
            },
            {
                id: 5,
                name: "Natural Makeup",
                description: "Soft, natural-looking makeup.",
                price: 140,
                durationMinutes: 60,
                isActive: true
            }
        ],

        staff: [
            {
                id: 1,
                fullName: "Maya",
                role: "Nails Artist",
                isAvailable: true
            },
            {
                id: 2,
                fullName: "Lina",
                role: "Nail Artist",
                isAvailable: true
            },
            {
                id: 4,
                fullName: "Raya",
                role: "Hair Artist",
                isAvailable: true
            }
        ],

        appointments: []
    };

    const store = {
        list(type) {
            if (!Array.isArray(state[type])) {
                throw new Error(`Unknown demo data type: ${type}`);
            }

            return state[type].map(item => ({ ...item }));
        },

        get(type, id) {
            const item = state[type]?.find(x => x.id === Number(id));
            return item ? { ...item } : null;
        },

        add(type, item) {
            if (!Array.isArray(state[type])) {
                throw new Error(`Unknown demo data type: ${type}`);
            }

            const nextId = state[type].reduce(
                (max, current) => Math.max(max, current.id || 0),
                0
            ) + 1;

            const newItem = {
                ...item,
                id: nextId
            };

            state[type].push(newItem);
            return { ...newItem };
        },

        update(type, id, changes) {
            const item = state[type]?.find(x => x.id === Number(id));

            if (!item) {
                return null;
            }

            Object.assign(item, changes, { id: item.id });
            return { ...item };
        },

        remove(type, id) {
            if (!Array.isArray(state[type])) {
                throw new Error(`Unknown demo data type: ${type}`);
            }

            const index = state[type].findIndex(
                item => item.id === Number(id)
            );

            if (index === -1) {
                return false;
            }

            state[type].splice(index, 1);
            return true;
        }
    };

    Object.defineProperty(window, "AurevaDemo", {
        value: Object.freeze(store),
        writable: false,
        configurable: false
    });
})();